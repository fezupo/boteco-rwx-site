# Assets públicos do Boteco RWX

## Hero oficial

O primeiro vertical slice espera o arquivo:

`public/assets/boteco-rwx-background.webp`

Origem: plano de fundo oficial usado nas transmissões do Boteco RWX.

Derivado recomendado para a V1:

- fonte: 1672x941 PNG (~3 MB)
- formato servido: WebP
- dimensões: 1440x810
- qualidade usada no derivado: 76
- tamanho observado: ~151 KB
- uso: Hero da Home

O PNG original não deve ser servido diretamente pela Home porque custa aproximadamente 3 MB e não entrega benefício proporcional nesse uso.

A versão WebP recebe overlays em CSS para preservar leitura e contraste. Em telas estreitas a mesma arte é reposicionada com `background-position`; um crop mobile dedicado só deve ser criado se teste real mostrar necessidade.

## Regra de asset

- otimizar antes de publicar;
- não enviar resolução muito maior do que a área real de exibição;
- manter contraste suficiente para texto e navegação;
- evitar duplicar o mesmo fundo em todas as seções;
- benchmark visual em desktop e celular manda mais do que dogma de formato.

## Pendência operacional

O conector GitHub usado durante o desenvolvimento atual escreve arquivos de texto, mas não envia o binário WebP. O arquivo otimizado já foi produzido e precisa ser colocado neste diretório antes do preview visual definitivo.
