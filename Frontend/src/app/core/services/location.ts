import { Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LocationsModel } from '../models/location';

@Injectable({
  providedIn: 'root'
})
export class Location {
     private apiUrl = 'https://enhanzer-assignment-api-d5dfasgpcncve4c4.southeastasia-01.azurewebsites.net/api/Location';

  constructor(private http: HttpClient) {}

  getLocations(): Observable<LocationsModel[]> {
    return this.http.get<LocationsModel[]>(this.apiUrl);
  }
}
