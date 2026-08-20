# Boteco RWX Site

Casa digital pública do Boteco RWX.

## Stack

- Astro
- TypeScript
- HTML semântico
- CSS moderno
- JavaScript somente quando houver interação real

## Filosofia

> **Magia é permitida. Desperdício não.**

O projeto segue a filosofia Carmack: simplicidade, modularidade, performance medida, código legível e complexidade somente quando houver necessidade real.

## Desenvolvimento

```bash
npm install
npm run dev
```

Validação local:

```bash
npm run check
npm run build
npm run budget
```

Preview do build:

```bash
npm run preview
```

## Asset visual principal

A Home espera o cenário oficial do Boteco em:

```text
public/assets/boteco-rwx-background.webp
```

O derivado recomendado é 1440x810 em WebP, aproximadamente 151 KB. O PNG original das transmissões não deve ser servido diretamente na Home.

## Fluxo

- `main`: linha estável
- `dev/*`: desenvolvimento
- mudanças entram por Pull Request
- o PR do primeiro slice permanece Draft até revisão visual e autorização explícita

A CI não roda a cada commit de DEV. Ela valida a abertura/reabertura do PR, a passagem para Ready for Review ou execução manual.

Quando executada, a CI:

1. checa Astro/TypeScript;
2. gera o build de produção;
3. mede o performance budget;
4. guarda o `dist/` como artefato temporário.

## Documentação

- `docs/LANDMARK_00.md`: baseline inicial
- `docs/PROJECT_STATUS.md`: checkpoint vivo
- `docs/CONTENT_GUIDE.md`: manutenção simples de conteúdo
