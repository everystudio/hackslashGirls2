REM # 環境によっては、ImageMagickのコマンドは「magick <cmd>」じゃないと動かない
setlocal enabledelayedexpansion

for /d %%f in (Level*) do (
  echo %%f
  cd %%f

  for %%g in (*.png) do (
      magick convert -resize x100 %%g %%g
  )
  cd ../
)

cd resizetemp
for %%g in (*.png) do (
    magick convert -resize x100 %%g %%g
)
cd ../


