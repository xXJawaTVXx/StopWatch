import { Component } from '@angular/core';
import { ImageService } from './services/image.service';
import { tap } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  public stopWatchTime: string = '-';
  public allowedTypes: string[] = ['image/jpeg', 'image/png'];

  public constructor(private readonly _imageService: ImageService) {
  }

  public async onFileSelected(event: any): Promise<void> {
    const uploadedImage: File = event.files[0];
    if (uploadedImage && this.allowedTypes.includes(uploadedImage.type)) {
      const base64String: string = await this._imageService.convertToBase64(uploadedImage);
      this._imageService.postImageToAnalyse(base64String).pipe(
        tap((res: string): string => this.stopWatchTime = res)
      ).subscribe();
    } else {
      console.error("Error while uploading: Type not allowed");
    }
  }
}
