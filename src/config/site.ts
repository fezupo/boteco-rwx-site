import { withBase } from '../utils/paths';

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
    { label: 'Início', href: withBase('/') },
    { label: 'Lives', href: withBase('/lives') },
    { label: 'Agenda', href: withBase('/agenda') },
    { label: 'Cortes', href: withBase('/cortes') },
    { label: 'A Mesa', href: withBase('/mesa') },
    { label: 'Sobre', href: withBase('/sobre') },
  ],
  external: externalLinks,
} as const;

export const activeExternalLinks = Object.values(site.external).filter(
  (link): link is ExternalLink & { url: string } => typeof link.url === 'string' && link.url.length > 0,
);
