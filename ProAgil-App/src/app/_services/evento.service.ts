import { Evento } from './../_models/Evento';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class EventoService {

  baseURL = 'http://localhost:50859/api/evento';

  constructor(private http : HttpClient) {
   }

  getAllEvento(): Observable<Evento[]> {
    return this.http.get<Evento[]>(this.baseURL);
  }

  getEventoByTema(tema: string): Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseURL}/getByTema/${tema}`);
  }

  getEventoById(id: number): Observable<Evento> {
    return this.http.get<Evento>(`${this.baseURL}/${id}`);
  }

  postUpload(file: File, name: string)
  {
    const fileToUpload = <File>file[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, name);

    return this.http.post(`${this.baseURL}/upload`, formData)
  }

  postEvento(evento: any) {
    return this.http.post(this.baseURL, evento);
  }

  putEvento(evento: Evento) {
    var url = `${this.baseURL}/editar`;
    return this.http.put(url, evento);
  }

  deleteEventoById(id: number) {
    var url = `${this.baseURL}/deletarById/${id}`;
    return this.http.delete(url);
  }

}//
