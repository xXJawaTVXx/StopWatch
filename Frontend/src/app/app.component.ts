import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  public stopWatchTime: string = '0:20:00:14';
  public allowedTypes: string[] = ['image/jpeg'];

  public onFileSelected(event: any): boolean {
    const uploadedImage: File = event.files[0];
    if (uploadedImage) {
      return true;
    }
    return false;
  }
}
