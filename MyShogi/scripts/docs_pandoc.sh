#!/bin/bash
# -*- coding: utf-8 -*-

# sudo apt install pandoc texlive-full python-pip
# pip install panflute

cd `dirname $0`

for dirname in ../docs/html ../docs/pdf; do
  if [ ! -e $dirname ]; then
    mkdir $dirname
  fi
done

for file in `find ../docs -name '*.md' -type f`; do
  echo $file

# HTML template:
#   - https://qiita.com/cawpea/items/cea1243e106ababd15e7
#   - https://github.com/cawpea/md2html-template-for-pandoc/blob/master/md2html.html
  pandoc -o ../docs/html/`basename ${file%.*}.html` $file -t html5 --toc -f markdown+emoji --template=./md2html.html5 --filter=./filter_html5.py --self-contained -s --mathjax="data:text/javascript,%28function%28_%29%7B_.setAttribute%28%27type%27%2C%27text%2Fjavascript%27%29%3B_.setAttribute%28%27src%27%2C%27https%3A%2F%2Fcdn.mathjax.org%2Fmathjax%2Flatest%2FMathJax.js%3Fconfig%3DTeX-AMS-MML_HTMLorMML%27%29%3Bdocument.getElementsByTagName%28%27head%27%29%5B0%5D.appendChild%28_%29%7D%29%28document.createElement%28%27script%27%29%29"

  pandoc -o ../docs/pdf/`basename ${file%.*}.pdf` $file -t latex --toc -f markdown+emoji --filter=./filter_pdf.py --latex-engine=xelatex -V documentclass=bxjsarticle -V classoption=pandoc -V linkcolor=blue -V geometry:a4paper -V geometry:margin=1in

done
