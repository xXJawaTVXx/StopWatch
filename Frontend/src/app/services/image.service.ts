import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  private readonly _headers = { 'content-type': 'application/json' };
  private _serviceUrl: string = "/stopwatch/";
  constructor(private readonly _httpClient: HttpClient) {

  }

  public postImageToAnalyse(image: string): Observable<string> {
    return this._httpClient.post<string>(environment.api.baseUri + this._serviceUrl + "analyse", JSON.stringify(image), { headers: this._headers, responseType: 'text' as 'json' });
  }

  public convertToBase64(file: File): Promise<string> {
    return new Promise((resolve, reject): void => {
      const reader: FileReader = new FileReader();

      reader.onload = (): void => {
        const result = reader.result as string;
        const base64String = result.split(',')[1];
        resolve(base64String);
      };
      reader.onerror = (error: ProgressEvent<FileReader>) => reject(`Error converting file to base64: ${ error }`);

      reader.readAsDataURL(file);
    });
  }
}
