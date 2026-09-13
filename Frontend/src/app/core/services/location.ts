import { Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LocationsModel } from '../models/location';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class Location {
     private apiUrl = `${environment.apiUrl}/Location`;

  constructor(private http: HttpClient) {}

  getLocations(): Observable<LocationsModel[]> {
    return this.http.get<LocationsModel[]>(this.apiUrl);
  }
}
