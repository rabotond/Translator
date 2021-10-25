import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BackendService {

  constructor(private httpClient: HttpClient) { }

  public TranslateText(inputText: string, srcLanguage: string, destLanguage: string) : Observable<string> {
    const headers = new HttpHeaders().append('content-type', 'application/json');
    const params = new HttpParams()
        .append('input', inputText)
        .append('sourceLanguage', srcLanguage)
        .append('targetLanguage', destLanguage);
    return this.httpClient.get<string>('http://localhost:5000/api/v1/Translator/Translate', { headers, params });
  }
}
