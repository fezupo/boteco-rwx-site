export type ExternalLink = {
  label: string;
  url: string | null;
};

export const site = {
  name: 'Boteco RWX',
  description: 'Games, anime, tokusatsu, música e conversa sem frescura.',
  slogan: 'Aqui o freguês não só assiste. Ele senta à mesa.',
  nav: [
    { label: 'Início', href: '/' },
    { label: 'Lives', href: '/lives' },
    { label: 'Agenda', href: '/agenda' },
    { label: 'Cortes', href: '/cortes' },
    { label: 'A Mesa', href: '/mesa' },
    { label: 'Sobre', href: '/sobre' },
  ],
  external: {
    youtube: { label: 'YouTube', url: null },
    instagram: { label: 'Instagram', url: null },
    tiktok: { label: 'TikTok', url: null },
    x: { label: 'X', url: null },
  } satisfies Record<string, ExternalLink>,
} as const;

export const activeExternalLinks = Object.values(site.external).filter(
  (link): link is ExternalLink & { url: string } => typeof link.url === 'string' && link.url.length > 0,
);
