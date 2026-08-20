import { readdir, stat } from 'node:fs/promises';
import { extname, join } from 'node:path';

const ROOT = new URL('../dist/', import.meta.url);
const LIMITS = {
  js: 120 * 1024,
  css: 220 * 1024,
};

const totals = { js: 0, css: 0 };

async function walk(path) {
  for (const name of await readdir(path)) {
    const full = join(path, name);
    const info = await stat(full);

    if (info.isDirectory()) {
      await walk(full);
      continue;
    }

    const ext = extname(name);
    if (ext === '.js') totals.js += info.size;
    if (ext === '.css') totals.css += info.size;
  }
}

await walk(ROOT);

const kb = (value) => `${(value / 1024).toFixed(1)} KB`;
console.log(`Client JS: ${kb(totals.js)} / ${kb(LIMITS.js)}`);
console.log(`CSS total: ${kb(totals.css)} / ${kb(LIMITS.css)}`);

const violations = [];
if (totals.js > LIMITS.js) violations.push(`JavaScript excedeu ${kb(LIMITS.js)}`);
if (totals.css > LIMITS.css) violations.push(`CSS excedeu ${kb(LIMITS.css)}`);

if (violations.length) {
  console.error(`Performance budget falhou: ${violations.join('; ')}`);
  process.exit(1);
}

console.log('Performance budget OK.');
