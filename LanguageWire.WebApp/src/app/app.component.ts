import { Component } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent {
    constructor(private http: HttpClient) {}

    displayedColumns: string[] = ['German', 'Spanish', 'English'];
    translatedText = '';
    public playerName: string = '';

    public TranslateText() {
        const headers = new HttpHeaders().append('content-type', 'application/json');
        const params = new HttpParams()
            .append('input', (<HTMLInputElement>document.getElementById('inputTextArea')).value)
            .append('targetLang', (<HTMLInputElement>document.getElementById('targetLang')).value);
        this.http.get<string>('http://localhost:5000/Translator', { headers, params }).subscribe({
            next: (data) => {
                this.translatedText = data;
            },
            error: (error) => {
                console.error('There was an error!', error);
            }
        });
    }
}
