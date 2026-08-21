const rawBase = import.meta.env.BASE_URL || '/';
const normalizedBase = rawBase === '/' ? '' : rawBase.replace(/\/$/, '');

export const withBase = (path = '/') => {
  if (/^(?:https?:|mailto:|tel:|#)/.test(path)) return path;

  const normalizedPath = path.startsWith('/') ? path : `/${path}`;
  if (normalizedPath === '/') return normalizedBase ? `${normalizedBase}/` : '/';

  return `${normalizedBase}${normalizedPath}`;
};
