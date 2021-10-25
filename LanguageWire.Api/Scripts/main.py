import sys
from translatorLib import google_translator

inputText = sys.argv[1]
sourceLand = sys.argv[2]
targetLang = sys.argv[3]

translator = google_translator()
translate_text = translator.translate(
    inputText, lang_tgt=targetLang, lang_src=sourceLand)
print(translate_text)
