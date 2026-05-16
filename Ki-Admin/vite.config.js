import { defineConfig } from 'vite';
import laravel from 'laravel-vite-plugin';
import tailwindcss from '@tailwindcss/vite';
import path from 'path';

export default defineConfig({
    plugins: [
        laravel({
            input: ['resources/css/app.css', 'resources/css/sidebar-override.css', 'resources/js/app.js', 'public/assets/scss/style.scss'],
            refresh: true,
        }),
        tailwindcss(),
    ],
    resolve: {
        alias: {
            '@scss': path.resolve(__dirname, 'public/assets/scss/app'),  // Define an alias for SCSS folder
        },
    }
});
