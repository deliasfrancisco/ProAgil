import { EventoService } from './../_services/evento.service';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { Evento } from '../_models/Evento';
import { ThrowStmt } from '@angular/compiler';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-eventos',
  templateUrl: './Eventos.component.html',
  styleUrls: ['./Eventos.component.scss']
})
export class EventosComponent implements OnInit {

  eventos: Evento[];
  eventosFiltrados: Evento[];
  imagemLargura  = 50;
  imagemMargem = 2;
  mostrarImagem = false;
  _filtroLista:string = '';
  modalRef: BsModalRef;


  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService
    )
    { }

  ngOnInit() {
    this.getEventos();
  }

  get filtroLista(): string{
    return this._filtroLista;
  }

  set filtroLista(value: string){
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista): null;
  }

  filtrarEventos(filtrarPor:string): Evento[]{
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(evento =>
        evento.tema.toLocaleLowerCase().indexOf(filtrarPor)
      );
  }

  openModal(template: TemplateRef<any>){
    this.modalRef = this.modalService.show(template);
  }

  alterarImagem(){
    this.mostrarImagem = !this.mostrarImagem;
  }

  getEventos(){
    this.eventoService.getAllEvento().subscribe(
      (_evento: Evento[]) => {
      this.eventos = _evento;
      this.eventosFiltrados = this.eventos;
      console.log(_evento);
    }, error => {
      console.log(error);

    });
  }
}
