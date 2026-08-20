export type ExternalLink = {
  label: string;
  url: string | null;
};

const externalLinks: Record<string, ExternalLink> = {
  youtube: { label: 'YouTube', url: null },
  instagram: { label: 'Instagram', url: null },
  tiktok: { label: 'TikTok', url: null },
  x: { label: 'X', url: null },
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
  external: externalLinks,
} as const;

export const activeExternalLinks = Object.values(site.external).filter(
  (link): link is { label: string; url: string } => Boolean(link.url),
);
