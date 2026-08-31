# BOTECO RWX SITE | PROJECT STATUS

**Atualizado:** 20/08/2026  
**Branch ativa:** `dev/vertical-slice-v1`  
**PR:** #1 Draft

## Estado atual

### Concluído

- Repositório separado da MESA VIP.
- Repositório público para permitir GitHub Pages no plano atual.
- `main` mantida como linha estável, sem merge do vertical slice.
- Branch `dev/vertical-slice-v1` mantém o protótipo ativo.
- Landmark 00 registrado.
- Astro + TypeScript + HTML semântico + CSS próprio.
- Home implementada.
- Páginas Lives, Agenda, Cortes, A Mesa, Sobre e 404 implementadas.
- Navegação desktop e mobile sem JavaScript obrigatório.
- `aria-current`, foco de teclado e suporte ao subpath do GitHub Pages.
- LIVE 05 cadastrada como próxima rodada.
- Hero reage ao estado da live sem JavaScript cliente.
- Links externos centralizados em configuração.
- Redes sem URL não aparecem publicamente.
- Visual arcade / boteco / neon / CRT.
- Plano de fundo oficial das transmissões adotado como ambiente visual do Hero.
- Asset oficial WebP versionado em `public/assets/boteco-rwx-background.webp`.
- CSS responsivo com overlay de contraste e reposicionamento mobile.
- SEO e metadata inicial.
- Favicon SVG e `robots.txt`.
- `prefers-reduced-motion` e `prefers-reduced-transparency` respeitados.
- View Transitions progressivas sem framework cliente obrigatório.
- `content-visibility` nas seções fora da primeira dobra.
- Guia de conteúdo em `docs/CONTENT_GUIDE.md`.
- Performance budget automatizado.
- Runtime Node 22.19+.
- Astro preparado para `/boteco-rwx-site/`.
- Helper `withBase()` aplicado a navegação, Hero, assets e links internos relevantes.
- GitHub Pages ativado manualmente com Source = GitHub Actions.
- CI da DEV separada do deploy de Pages.
- Workflow da `main` preparado para publicar automaticamente o conteúdo da DEV depois de CI verde, sem merge do produto.

## Decisões congeladas por enquanto

- Carmack como filosofia de engenharia.
- Regra: **Magia é permitida. Desperdício não.**
- Astro é a base inicial.
- TypeScript para lógica e dados.
- CSS próprio para identidade visual.
- Sem Tailwind por padrão.
- Sem SPA por padrão.
- Sem CMS, banco ou backend sem necessidade real.
- YouTube é o único destino externo previsto inicialmente.
- Outras redes ficam preparadas, porém invisíveis até existirem.
- Will e KV entram no processo quando houver experiência concreta para avaliar.
- O fundo oficial ambienta o Hero; as seções seguintes usam superfícies derivadas mais leves.
- PR #1 permanece Draft e o vertical slice não é mergeado sem autorização explícita.

## Primeiro vertical slice

Objetivo: a pessoa abrir o site em celular ou desktop e entender rapidamente:

1. que entrou no Boteco RWX;
2. o espírito do canal;
3. qual é a próxima rodada;
4. como explorar o restante.

O site continua funcional mesmo sem JavaScript obrigatório.

## Asset oficial

O fundo do Hero está versionado em:

`public/assets/boteco-rwx-background.webp`

A inclusão foi feita como blob Git binário real, sem base64 persistido e sem depender do ZIP offline.

## Publicação do protótipo

Destino do GitHub Pages:

`https://fezupo.github.io/boteco-rwx-site/`

Arquitetura de homologação:

1. push em `dev/vertical-slice-v1` dispara `site-ci`;
2. CI executa Astro/TypeScript, build e performance budget;
3. se a CI terminar verde, o workflow `pages-preview` que vive na `main` é disparado por `workflow_run`;
4. o workflow da `main` faz checkout da DEV, recompila o mesmo protótipo e publica no ambiente `github-pages`;
5. o produto continua fora da `main` até autorização de merge.

Essa arquitetura existe porque o ambiente `github-pages` criado pelo GitHub protege por padrão a branch principal. O deploy é autorizado pela `main`, mas o conteúdo publicado continua vindo da DEV.

## Validação

Últimas validações do código do protótipo: **verdes**.

Incluem:

- Astro/TypeScript check: sucesso;
- build de produção: sucesso;
- 7 rotas estáticas geradas;
- performance budget: sucesso;
- artefato estático: sucesso;
- Node 22.19 alinhado.

## Próximos passos

1. Confirmar o primeiro deploy automático da nova arquitetura.
2. Abrir a URL do protótipo em desktop e celular.
3. Ajustar texto, densidade, crop e hierarquia pela sensação real em tela.
4. Cadastrar a URL oficial do YouTube quando definida para o site.
5. Inserir Will e KV para feedback quando o protótipo estiver maduro o suficiente.

## Regra de merge

PR #1 permanece Draft. Não fazer merge para `main` antes de revisão e autorização explícita.
