import { defineConfig } from 'astro/config';

const isPagesBuild = process.env.GITHUB_PAGES === 'true';
const repository = process.env.GITHUB_REPOSITORY?.split('/')[1] ?? 'boteco-rwx-site';

export default defineConfig({
  output: 'static',
  site: isPagesBuild ? 'https://fezupo.github.io' : undefined,
  base: isPagesBuild ? `/${repository}` : '/',
});
