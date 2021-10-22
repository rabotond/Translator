import sys
from translatorLib import google_translator  

inputText = sys.argv[1]
targetLang = sys.argv[2]

# print("input is", inputText, " target language is ", targetLang)

translator = google_translator()  
translate_text = translator.translate(inputText,lang_tgt=targetLang)  
print(translate_text)