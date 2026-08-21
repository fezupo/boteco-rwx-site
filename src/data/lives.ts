export type LiveStatus = 'upcoming' | 'live' | 'finished';

export type Live = {
  number: number;
  title: string;
  kicker: string;
  description: string;
  status: LiveStatus;
  youtubeUrl: string | null;
  tags: string[];
};

export const featuredLive: Live = {
  number: 5,
  title: 'Final Fight vs Streets of Rage',
  kicker: 'Qual envelheceu melhor?',
  description:
    'Dois clássicos, duas filosofias e uma mesa pronta para discutir gameplay, ritmo, identidade e nostalgia.',
  status: 'upcoming',
  youtubeUrl: null,
  tags: ['Games', 'Beat ’em up', 'Arcade', 'Mega Drive'],
};
