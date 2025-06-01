/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './**/*.razor',
    './wwwroot/index.html',
  ],
  theme: {
    extend: {
      colors: {
        'primary': '#0078D4', // Windows Blue
        'primary-hover': '#005A9E',
        'primary-active': '#004578',
        'surface': '#FFFFFF',
        'background': '#EFF6FC', // Light background
        'text-primary': '#201F1E',
        'text-secondary': '#605E5C',
        'text-on-primary': '#FFFFFF',
        'border-color': '#C8C6C4', // Default border
        'border-hover': '#7A7A7A',
        'focus-ring': '#0078D4',
        'error': '#A80000',
        'success': '#107C10',
        'sidebar-bg': '#F3F2F1', // Slightly off-white for sidebar
        'sidebar-text': '#201F1E',
        'sidebar-item-hover-bg': '#E1E1E1',
        'sidebar-item-active-bg': '#C7E0F4',
        'sidebar-item-active-text': '#005A9E',

      },
      fontFamily: {
        sans: ['Open Sans', 'ui-sans-serif', 'system-ui', '-apple-system', 'BlinkMacSystemFont', '"Segoe UI"', 'Roboto', '"Helvetica Neue"', 'Arial', '"Noto Sans"', 'sans-serif', '"Apple Color Emoji"', '"Segoe UI Emoji"', '"Segoe UI Symbol"', '"Noto Color Emoji"'],
      },
    },
  },
  plugins: [
    require('@tailwindcss/forms'),
  ],
}