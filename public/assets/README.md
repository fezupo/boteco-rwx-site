# Assets públicos

## Fundo oficial do Boteco RWX

Arquivo esperado pelo primeiro vertical slice:

`public/assets/boteco-rwx-background.webp`

Fonte visual: plano de fundo oficial usado nas transmissões do Boteco RWX, fornecido por Felipe em 20/08/2026.

### Derivação recomendada para produção

- fonte: 1672x941 PNG (~3 MB)
- derivado WebP: 1440x810
- qualidade WebP: 76
- tamanho observado: ~151 KB
- não fazer upscale

O original pesado não deve ser servido diretamente na Home. A versão WebP é usada como ambiente do Hero e recebe overlays em CSS para preservar leitura e contraste.

Em telas estreitas a mesma arte é reposicionada com `background-position`; um crop mobile dedicado só deve ser criado se teste real mostrar necessidade.
