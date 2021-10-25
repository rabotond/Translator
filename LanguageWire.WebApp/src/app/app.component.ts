import { Component } from '@angular/core';
import { BackendService } from 'src/backend.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent {
    constructor(private backendService: BackendService) {}

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
       this.backendService.
       TranslateText(this.inputText, this.selectedSourceLanguage, this.selectedTargetLanguage)
       .subscribe(x => this.translatedText = x);
    }
}
