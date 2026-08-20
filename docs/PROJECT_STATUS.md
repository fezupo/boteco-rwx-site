# BOTECO RWX SITE | PROJECT STATUS

**Atualizado:** 20/08/2026  
**Branch ativa:** `dev/vertical-slice-v1`  
**PR:** #1 Draft

## Estado atual

### Concluído

- Repositório privado separado da MESA VIP.
- `main` inicializada como linha estável.
- Branch de desenvolvimento criada.
- Landmark 00 registrado.
- Astro + TypeScript + CSS moderno.
- Home implementada.
- Páginas Lives, Agenda, Cortes, A Mesa, Sobre e 404 implementadas.
- Navegação desktop e mobile sem JavaScript obrigatório.
- Navegação com `aria-current`, foco de teclado e suporte ao subpath do GitHub Pages.
- LIVE 05 cadastrada como próxima rodada.
- Hero reage ao estado da live sem JavaScript cliente.
- Links externos centralizados em configuração.
- Redes sem URL não aparecem publicamente.
- Visual arcade / boteco / neon / CRT.
- Plano de fundo oficial das transmissões adotado como ambiente visual do Hero.
- Asset oficial WebP versionado de verdade no Git em `public/assets/boteco-rwx-background.webp`.
- Derivado leve de protótipo em 1024 px usado no repositório para reduzir transferência.
- CSS responsivo preparado para o asset oficial com overlay de contraste e reposicionamento mobile.
- Metadados básicos de SEO e compartilhamento adicionados.
- Favicon SVG leve adicionado.
- `robots.txt` mínimo adicionado.
- `prefers-reduced-motion` e `prefers-reduced-transparency` respeitados.
- View Transitions progressivas adicionadas sem framework cliente obrigatório.
- Seções fora da primeira dobra usam `content-visibility` para reduzir trabalho inicial do navegador.
- Guia de manutenção de conteúdo criado em `docs/CONTENT_GUIDE.md`.
- Performance budget automatizado para JavaScript e CSS.
- CI gera artefato estático de build quando executada.
- Runtime da CI alinhado para Node 22.19+.
- Configuração Astro preparada para GitHub Pages em `/boteco-rwx-site/`.
- Helper `withBase()` aplicado a navegação, Hero, assets e links internos relevantes.
- Workflow `.github/workflows/pages.yml` criado para build, budget, artifact e deploy no GitHub Pages.
- Checkpoint anterior validado com sucesso: Astro/TypeScript, build de produção, performance budget e artefato estático.
- Protótipo navegável offline montado a partir do build verde e do fundo oficial otimizado.

### Decisões congeladas por enquanto

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
- O fundo oficial não será repetido ao longo de toda a página; ele ambienta o Hero e as seções seguintes usam superfícies derivadas mais leves.
- A `main` não recebe o vertical slice enquanto o PR continuar Draft.

## Primeiro vertical slice

Objetivo: a pessoa abrir o site em celular ou desktop e entender rapidamente:

1. que entrou no Boteco RWX;
2. o espírito do canal;
3. qual é a próxima rodada;
4. como explorar o restante.

O site continua funcional mesmo sem JavaScript obrigatório.

## Asset oficial

O fundo do Hero está versionado na branch no caminho:

`public/assets/boteco-rwx-background.webp`

A inclusão foi feita como blob Git binário real, sem base64 persistido, sem chunk temporário e sem depender do ZIP offline.

## GitHub Pages

O repositório e o código já estão preparados para publicação em:

`https://fezupo.github.io/boteco-rwx-site/`

O workflow de Pages chegou a executar no GitHub Actions, mas foi interrompido no passo `Configure GitHub Pages` porque o recurso Pages ainda não foi habilitado uma vez nas configurações do repositório.

Bloqueio externo atual:

- GitHub Pages precisa ser habilitado em **Settings > Pages > Build and deployment > Source: GitHub Actions**.
- Depois dessa ativação única, o workflow já versionado pode ser reexecutado sem alteração estrutural do projeto.

## Validação

Último checkpoint completo anterior à tentativa de Pages: **verde**.

Incluiu:

- Astro/TypeScript check: sucesso, sem erros;
- build de produção: sucesso;
- 7 rotas estáticas geradas;
- performance budget: sucesso;
- artefato estático gerado;
- Node 22.19 alinhado com o ecossistema atual.

A execução de Pages mais recente não chegou ao check/build porque o próprio GitHub encerrou antes, durante a configuração de Pages desabilitado. Isso não representa falha do código do site.

## Próximos passos

1. Habilitar GitHub Pages uma única vez usando Source = GitHub Actions.
2. Reexecutar o workflow `deploy-pages-preview`.
3. Confirmar build, budget e deploy verdes.
4. Abrir a URL do protótipo em desktop e celular.
5. Ajustar texto, densidade, crop e hierarquia pela sensação real em tela.
6. Cadastrar a URL oficial do YouTube quando definida para o site.
7. Inserir Will e KV para feedback quando o protótipo estiver maduro o suficiente.

## Regra de merge

PR #1 permanece Draft. Não fazer merge para `main` antes de revisão e autorização explícita.
