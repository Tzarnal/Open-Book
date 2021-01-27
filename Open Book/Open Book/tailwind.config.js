module.exports = {
    theme: {
        extend: {
            colors: {
            }
        }
    },
    variants: {
        extend: {
            display: ['group-hover'],
        }
    },
    plugins: [        
        require('@tailwindcss/forms'),
    ],
}