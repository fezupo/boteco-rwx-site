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
- Páginas Lives, Agenda, Cortes, A Mesa e Sobre implementadas.
- Navegação desktop e mobile sem JavaScript obrigatório.
- LIVE 05 cadastrada como próxima rodada.
- Links externos centralizados em configuração.
- Redes sem URL não aparecem publicamente.
- Visual inicial arcade / boteco / neon / CRT.
- Responsividade inicial.
- `prefers-reduced-motion` respeitado.
- CI configurada para Pull Request e execução manual.
- Plano de fundo oficial das transmissões adotado como ambiente visual do Hero.
- CSS responsivo preparado para o asset oficial com overlay de contraste e reposicionamento mobile.
- Derivação WebP definida em 1440x810 (~151 KB), evitando servir o PNG original de ~3 MB.

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

## Primeiro vertical slice

Objetivo: a pessoa abrir o site em celular ou desktop e entender rapidamente:

1. que entrou no Boteco RWX;
2. o espírito do canal;
3. qual é a próxima rodada;
4. como explorar o restante.

## Pendência de asset

O código espera o arquivo:

`public/assets/boteco-rwx-background.webp`

O derivado WebP já foi preparado fora do repositório. O conector GitHub usado no desenvolvimento atual não envia binários, portanto esse único arquivo precisa ser adicionado ao caminho acima por upload de arquivo antes do preview visual definitivo.

## Próximos passos

1. Adicionar `public/assets/boteco-rwx-background.webp` ao PR #1.
2. Confirmar CI verde do PR #1.
3. Revisar visual real da Home com o fundo oficial.
4. Ajustar texto, densidade, crop e hierarquia conforme sensação em desktop e celular.
5. Cadastrar URL oficial do YouTube quando definida para o site.
6. Decidir hospedagem e domínio somente depois do slice aprovado.
7. Inserir Will e KV para feedback quando Felipe considerar o protótipo maduro o suficiente.

## Regra de merge

PR #1 permanece Draft. Não fazer merge para `main` antes de revisão e autorização explícita.
