# Boteco RWX Site | Guia rápido de conteúdo

Este arquivo existe para que atualizar o site não vire trabalho de programador quando o conteúdo for simples.

## Próxima live

Editar `src/data/lives.ts`.

Campos atuais:

- `number`: número da live
- `title`: título curto exibido no site
- `kicker`: pergunta ou gancho da rodada
- `description`: resumo curto
- `status`: `upcoming`, `live` ou `finished`
- `youtubeUrl`: URL oficial ou `null`
- `tags`: temas da rodada

Se `youtubeUrl` for `null`, o site não deve inventar link.

## Integrantes da Mesa

Editar `src/data/crew.ts`.

A representação pública dos integrantes deve ser revisada pela Mesa antes da publicação definitiva.

## Temas

Editar `src/data/topics.ts`.

A regra é manter poucos temas fortes. Não criar categoria só porque um assunto apareceu uma vez.

## Links externos

Editar `src/config/site.ts`.

Uma rede sem URL válida permanece invisível. Não usamos placeholders públicos do tipo “em breve”.

## Imagens

Assets públicos ficam em `public/assets/`.

Preferências:

1. WebP ou AVIF para fotografias e fundos.
2. SVG para ícones e marcas vetoriais simples.
3. Imagens dimensionadas para o uso real, sem enviar 4K para uma área de 600 px.
4. Qualidade visual deve ser avaliada em tela real, não só pelo tamanho do arquivo.

## Regra Carmack

Antes de adicionar ferramenta, plugin ou painel para resolver uma atualização, perguntar:

> Isso é realmente mais simples do que editar o dado diretamente?

Se a resposta for não, o código continua simples.
