# BOTECO RWX SITE | LANDMARK 00

**Data:** 20/08/2026  
**Status:** Baseline inicial  
**Escopo:** Produto público Web

## 1. Propósito

Criar a casa digital pública do Boteco RWX: simples, cativante, rápida e reconhecível em qualquer dispositivo.

O site não substitui o YouTube. Ele organiza a identidade, as lives, a agenda, os cortes e a apresentação da Mesa.

## 2. Princípio mestre

> **Magia é permitida. Desperdício não.**

A estética pode ter personalidade, movimento, neon, CRT e microinterações desde que isso não comprometa performance, legibilidade, acessibilidade, estabilidade ou manutenção.

## 3. Filosofia Carmack

- Resolver com menos código e menos estado quando possível.
- Não adicionar dependência sem problema real para resolver.
- Medir performance, não presumir.
- JavaScript apenas onde houver interação real.
- Código legível vence código esperto.
- Mobile é ambiente de primeira classe.
- Elementos secundários podem falhar sem quebrar a navegação principal.
- Carregar recurso somente quando necessário.
- Remover código morto quando uma direção de produto substitui a anterior.
- Decisões técnicas podem mudar quando benchmarks ou experiência real mostrarem caminho melhor.

## 4. Stack inicial

- Astro
- TypeScript
- HTML semântico
- CSS moderno
- Conteúdo local tipado
- Build estático

Frameworks adicionais, CMS, banco, bibliotecas de animação ou backend entram somente se uma necessidade concreta justificar.

## 5. V1

- Home
- Lives
- Agenda
- Cortes
- A Mesa
- Sobre
- 404 coerente com a identidade
- SEO básico
- Responsividade desktop/tablet/mobile
- Estrutura para links externos configuráveis
- Performance budget automatizado
- Build estático exportável

## 6. Links externos

YouTube é o único destino externo previsto inicialmente.

Outras redes ficam preparadas em configuração, porém **não são renderizadas quando não houver URL válida**. Nada de placeholders públicos para redes inexistentes.

## 7. Direção visual

- boteco digital
- cenário oficial das transmissões como ambiente do Hero
- arcade / neon / CRT como linguagem, não como ruído
- madeira escura e luz quente como contraponto
- vermelho, âmbar e azul como acentos
- alta legibilidade
- personalidade forte sem transformar a navegação em thumbnail
- seções internas mais limpas que o Hero para preservar leitura

## 8. Performance e magia

Magia permitida na V1:

- glow e contraste em CSS;
- scanline leve;
- microinterações;
- View Transitions progressivas sem framework cliente;
- estados visuais da live gerados no build.

Limites iniciais de build:

- JavaScript cliente total: 120 KB no máximo;
- CSS total: 220 KB no máximo.

Os limites são guardrails iniciais, não dogma. Mudam somente quando necessidade real e benchmark justificarem.

## 9. Não escopo inicial

- login
- fórum
- comentários próprios
- loja
- CMS pesado
- banco de dados sem necessidade
- SPA por padrão
- animação pesada sem benefício mensurável
- redes sociais inexistentes na interface

## 10. Governança

A baseline inicial nasce com Felipe + Ayame em modo de laboratório. Will e KV entram no processo quando houver uma experiência concreta para avaliar. Identidade coletiva, representação dos integrantes e evolução editorial devem receber feedback da Mesa antes de produção pública.

## 11. Critério do primeiro vertical slice

Abrir o site em celular ou desktop e, em poucos segundos, entender:

1. onde a pessoa caiu;
2. o que é o Boteco RWX;
3. qual é a próxima rodada;
4. onde continuar explorando.

O primeiro slice deve funcionar sem JavaScript obrigatório.

## 12. Regra de merge

A `main` é linha estável. O primeiro vertical slice permanece em PR Draft até revisão visual e autorização explícita para avançar.
