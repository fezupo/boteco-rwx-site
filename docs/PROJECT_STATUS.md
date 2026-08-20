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
- Navegação com `aria-current` e foco de teclado.
- LIVE 05 cadastrada como próxima rodada.
- Hero reage ao estado da live sem JavaScript cliente.
- Links externos centralizados em configuração.
- Redes sem URL não aparecem publicamente.
- Visual arcade / boteco / neon / CRT.
- Plano de fundo oficial das transmissões adotado como ambiente visual do Hero.
- CSS responsivo preparado para o asset oficial com overlay de contraste e reposicionamento mobile.
- Derivação WebP definida em 1440x810 (~151 KB), evitando servir o PNG original de ~3 MB.
- Metadados básicos de SEO e compartilhamento adicionados.
- Favicon SVG leve adicionado.
- `robots.txt` mínimo adicionado.
- `prefers-reduced-motion` e `prefers-reduced-transparency` respeitados.
- View Transitions progressivas adicionadas sem framework cliente.
- Seções fora da primeira dobra usam `content-visibility` para reduzir trabalho inicial do navegador.
- Guia de manutenção de conteúdo criado em `docs/CONTENT_GUIDE.md`.
- Performance budget automatizado para JavaScript e CSS.
- CI gera artefato estático de build quando executada.
- CI configurada para não rodar em cada push de DEV, evitando flood de notificações.
- Uma execução anterior da CI do PR concluiu com sucesso antes do pacote atual de refinamentos.

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

## Pendência de asset

O código espera o arquivo:

`public/assets/boteco-rwx-background.webp`

O derivado WebP já foi preparado fora do repositório. O conector GitHub usado no desenvolvimento atual não envia binários, portanto esse arquivo precisa ser adicionado ao caminho acima antes do preview visual definitivo.

Sem o arquivo, a Home ainda funciona usando as camadas de fallback, porém sem o cenário oficial das transmissões.

## Validação

A CI inicial do PR já concluiu com sucesso em um checkpoint anterior.

Depois dos refinamentos atuais, a CI não é disparada em todo commit. A próxima validação completa deve ser feita conscientemente quando o slice estiver pronto para revisão, preservando o fluxo sem flood de e-mail.

A validação inclui:

- Astro/TypeScript check;
- build de produção;
- performance budget;
- geração de artefato estático `dist/`.

## Próximos passos

1. Adicionar `public/assets/boteco-rwx-background.webp` ao PR #1.
2. Executar a validação completa do checkpoint atual.
3. Abrir o build real em desktop e celular.
4. Ajustar texto, densidade, crop e hierarquia pela sensação em tela.
5. Cadastrar a URL oficial do YouTube quando definida para o site.
6. Decidir hospedagem e domínio somente depois do slice aprovado.
7. Inserir Will e KV para feedback quando Felipe considerar o protótipo maduro o suficiente.

## Regra de merge

PR #1 permanece Draft. Não fazer merge para `main` antes de revisão e autorização explícita.
