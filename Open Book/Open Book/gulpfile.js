const gulp = require('gulp');
const postcss = require('gulp-postcss');
const cleanCSS = require('gulp-clean-css');
const purgecss = require('gulp-purgecss');

gulp.task('css:dev', () => {
    return gulp.src('./Styles/tailwind.css')
        .pipe(postcss([
            require('precss'),
            require('tailwindcss'),
            require('autoprefixer')
        ]))
        .pipe(gulp.dest('./wwwroot/css/'));
});

gulp.task('css:prod', () => {
    return gulp.src('./Styles/tailwind.css')
        .pipe(postcss([
            require('precss'),
            require('tailwindcss'),
            require('autoprefixer')
        ]))
        .pipe(purgecss({
            content: ['**/*.html', '**/*.razor'],
            extractors: [
                {
                    extractor: (content) => {
                        // fix for escaped tailwind prefixes (sm:, lg:, etc) and .5's
                        return content.match(/[A-Za-z0-9-_:\.\/]+/g) || []
                    },
                    extensions: ['css', 'html', 'razor'],
                },
            ],
        }))
        .pipe(cleanCSS({ level: 2 }))
        .pipe(gulp.dest('./wwwroot/css/'));
});