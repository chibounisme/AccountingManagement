using AccountingManagement.Application.Abstractions;
using AccountingManagement.Application.Companies.DTOs;
using AccountingManagement.Domain.Entities;
using AccountingManagement.Domain.Interfaces;
using AccountingManagement.Domain.Exceptions;
// using AutoMapper;

namespace AccountingManagement.Application.Companies.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUserRepository _userRepository; // To fetch accountant names
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeProvider _dateTimeProvider;
    // private readonly IMapper _mapper;

    public CompanyService(
        ICompanyRepository companyRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService,
        IDateTimeProvider dateTimeProvider
        // IMapper mapper
        )
    {
        _companyRepository = companyRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _dateTimeProvider = dateTimeProvider;
        // _mapper = mapper;
    }

    // Manual mapping for now
    private async Task<CompanyDto> MapToDto(Company company)
    {
        string? accountantFullName = null;
        if (company.AccountantId.HasValue)
        {
            var accountant = await _userRepository.GetByIdAsync(company.AccountantId.Value);
            accountantFullName = accountant?.FullName;
        }
        return new CompanyDto
        {
            Id = company.Id,
            Name = company.Name,
            LegalName = company.LegalName,
            TaxIdentificationNumber = company.TaxIdentificationNumber,
            TradeRegisterNumber = company.TradeRegisterNumber,
            Address = company.Address,
            City = company.City,
            PostalCode = company.PostalCode,
            Country = company.Country,
            PhoneNumber = company.PhoneNumber,
            Email = company.Email,
            ActivityCode = company.ActivityCode,
            LegalForm = company.LegalForm,
            CompanyCreationDate = company.CompanyCreationDate,
            AccountantId = company.AccountantId,
            AccountantFullName = accountantFullName,
            CreatedDate = company.CreatedDate,
            LastModifiedDate = company.LastModifiedDate
        };
    }

    private async Task<List<CompanyDto>> MapToDtoList(List<Company> companies)
    {
        var dtos = new List<CompanyDto>();
        foreach(var company in companies)
        {
            dtos.Add(await MapToDto(company));
        }
        return dtos;
    }


    public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto createCompanyDto)
    {
        var company = new Company(
            createCompanyDto.Name,
            createCompanyDto.TaxIdentificationNumber,
            createCompanyDto.LegalForm,
            createCompanyDto.CompanyCreationDate
        )
        {
            LegalName = createCompanyDto.LegalName,
            TradeRegisterNumber = createCompanyDto.TradeRegisterNumber,
            Address = createCompanyDto.Address,
            City = createCompanyDto.City,
            PostalCode = createCompanyDto.PostalCode,
            Country = createCompanyDto.Country,
            PhoneNumber = createCompanyDto.PhoneNumber,
            Email = createCompanyDto.Email,
            ActivityCode = createCompanyDto.ActivityCode,
            CreatedDate = _dateTimeProvider.UtcNow,
            LastModifiedDate = _dateTimeProvider.UtcNow,
            CreatedBy = _currentUserService.Username
        };

        if (_currentUserService.UserRole == Domain.Enums.UserRole.Admin && createCompanyDto.AccountantId.HasValue)
        {
            company.AssignAccountant(createCompanyDto.AccountantId.Value);
        }
        else if (_currentUserService.UserRole == Domain.Enums.UserRole.Accountant && _currentUserService.UserId.HasValue)
        {
            company.AssignAccountant(_currentUserService.UserId.Value);
        }
        
        var newCompany = await _companyRepository.AddAsync(company);
        return await MapToDto(newCompany);
    }

    public async Task DeleteCompanyAsync(int id)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        if (company == null) throw new EntityNotFoundException(nameof(Company), id);

        // Authorization: Ensure current user can delete this company
        if (_currentUserService.UserRole == Domain.Enums.UserRole.Accountant && company.AccountantId != _currentUserService.UserId)
        {
            throw new UnauthorizedAccessException("Accountants can only delete companies assigned to them.");
        }
        
        await _companyRepository.DeleteAsync(id);
    }

    public async Task<List<CompanyDto>> GetAllCompaniesAsync()
    {
        // This should typically be restricted or paginated in a real app
        // For admin use or small datasets.
        if (_currentUserService.UserRole != Domain.Enums.UserRole.Admin)
        {
            throw new UnauthorizedAccessException("Only admins can view all companies.");
        }
        var companies = await _companyRepository.GetAllAsync();
        return await MapToDtoList(companies);
    }

    public async Task<List<CompanyDto>> GetCompaniesForCurrentUserAsync()
    {
        if (!_currentUserService.IsAuthenticated || !_currentUserService.UserId.HasValue)
        {
            return new List<CompanyDto>();
        }

        List<Company> companies;
        if (_currentUserService.UserRole == Domain.Enums.UserRole.Admin)
        {
            companies = await _companyRepository.GetAllAsync();
        }
        else if (_currentUserService.UserRole == Domain.Enums.UserRole.Accountant)
        {
            companies = await _companyRepository.GetByAccountantIdAsync(_currentUserService.UserId.Value);
        }
        else
        {
            return new List<CompanyDto>(); // Or throw
        }
        return await MapToDtoList(companies);
    }


    public async Task<CompanyDto?> GetCompanyByIdAsync(int id)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        if (company == null) return null;

        // Authorization check
        if (_currentUserService.UserRole == Domain.Enums.UserRole.Accountant && company.AccountantId != _currentUserService.UserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to view this company.");
        }
        return await MapToDto(company);
    }

    public async Task UpdateCompanyAsync(UpdateCompanyDto updateCompanyDto)
    {
        var company = await _companyRepository.GetByIdAsync(updateCompanyDto.Id);
        if (company == null) throw new EntityNotFoundException(nameof(Company), updateCompanyDto.Id);

        // Authorization check
        if (_currentUserService.UserRole == Domain.Enums.UserRole.Accountant && company.AccountantId != _currentUserService.UserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this company.");
        }

        company.Name = updateCompanyDto.Name;
        company.LegalName = updateCompanyDto.LegalName;
        company.TaxIdentificationNumber = updateCompanyDto.TaxIdentificationNumber;
        company.TradeRegisterNumber = updateCompanyDto.TradeRegisterNumber;
        company.Address = updateCompanyDto.Address;
        company.City = updateCompanyDto.City;
        company.PostalCode = updateCompanyDto.PostalCode;
        company.Country = updateCompanyDto.Country;
        company.PhoneNumber = updateCompanyDto.PhoneNumber;
        company.Email = updateCompanyDto.Email;
        company.ActivityCode = updateCompanyDto.ActivityCode;
        company.LegalForm = updateCompanyDto.LegalForm;
        company.CompanyCreationDate = updateCompanyDto.CompanyCreationDate;
        
        if (_currentUserService.UserRole == Domain.Enums.UserRole.Admin)
        {
            if (updateCompanyDto.AccountantId.HasValue)
                company.AssignAccountant(updateCompanyDto.AccountantId.Value);
            else
                company.UnassignAccountant();
        }
        // Accountants cannot reassign companies via this method directly.

        company.LastModifiedDate = _dateTimeProvider.UtcNow;
        company.LastModifiedBy = _currentUserService.Username;

        await _companyRepository.UpdateAsync(company);
    }
}