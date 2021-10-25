import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent {
    constructor(private http: HttpClient) {}

    displayedColumns: string[] = ['German', 'Spanish', 'English'];
    translatedText = '';

    selectedSourceLanguage!: string;
    selectedTargetLanguage!: string;
    inputText!: string;

    languages = [
        { name: 'German', value: 'de' },
        { name: 'French', value: 'fr' },
        { name: 'English', value: 'en' }
    ];

    public TranslateText() {
        const headers = new HttpHeaders().append('content-type', 'application/json');
        const params = new HttpParams()
            .append('input', this.inputText)
            .append('sourceLanguage', this.selectedSourceLanguage)
            .append('targetLanguage', this.selectedTargetLanguage);
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
