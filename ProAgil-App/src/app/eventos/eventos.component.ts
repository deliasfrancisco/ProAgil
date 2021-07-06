import { EventoService } from './../_services/evento.service';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { Evento } from '../_models/Evento';
import { ThrowStmt } from '@angular/compiler';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {BsLocaleService } from 'ngx-bootstrap/datepicker';
import {ptBrLocale } from 'ngx-bootstrap/locale';
import {defineLocale} from 'ngx-bootstrap/chronos';
defineLocale('pt-br', ptBrLocale);

@Component({
  selector: 'app-eventos',
  templateUrl: './Eventos.component.html',
  styleUrls: ['./Eventos.component.scss']
})
export class EventosComponent implements OnInit {

  eventosFiltrados: Evento[];
  eventos: Evento[];
  evento: Evento;
  imagemLargura  = 50;
  imagemMargem = 2;
  mostrarImagem = false;
  _filtroLista:string = '';
  public registerForm: FormGroup;
  modoSalvar = 'post';
  bodyDeletarEvento = '';

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private formBuilder: FormBuilder,
    private localeService: BsLocaleService
    )
    {
      this.localeService.use('pt-br');
      this.registerForm = this.formBuilder.group({
        Tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
        Local: ['', Validators.required],
        DataEvento: ['', Validators.required],
        QtdPessoas: ['', [Validators.required, Validators.max(1200)]],
        Telefone: ['', Validators.required],
        Email: ['', [Validators.required, Validators.email]],
        ImagemUrl: ['']
      });
    }

  ngOnInit() {
    this.getEventos();
  }

  salvarAlteracao(template: any){
  if(this.registerForm.valid){
    let eventoId = this.evento['eventoId'];
    if(this.modoSalvar === 'post'){
      this.evento = Object.assign({}, this.registerForm.value);
      console.log(this.evento);
      this.eventoService.postEvento(this.evento)
      .subscribe(() => {
          template.hide();
          this.getEventos();
        }, error => {
          console.log(error);
        }
        );
      }
      else{
        this.evento = Object.assign({EventoId: eventoId}, this.registerForm.value);
        this.eventoService.putEvento(this.evento).subscribe(
          () => {
            template.hide();
            this.getEventos();
          }, error => {
            console.log(error);
          }
        );
      }
    }
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
    return this.eventos.filter(
      evento => evento.Tema.toLocaleLowerCase().indexOf(filtrarPor)
      );
  }

  editarEvento(evento: any, template: any){
    this.modoSalvar = 'put';
    this.openModal(template);
    this.evento = evento;

    this.registerForm.patchValue({
      Tema: evento.tema,
      Local: evento.local,
      DataEvento: evento.dataEvento,
      Telefone: evento.telefone,
      Email: evento.email,
      ImagemUrl: evento.imagemUrl
    });
    console.log(this.registerForm.controls);
  }

  novoEvento(template: any){
    this.modoSalvar = 'post';
    this.openModal(template);
  }

  openModal(template: any){
    this.registerForm.reset();
    template.show(template);
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

  excluirEvento(evento: any, template: any) {
    this.openModal(template);
    this.evento = evento;
    this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${evento.tema}, Código: ${evento.eventoId}`;
  }

  confirmeDelete(template: any) {
    this.eventoService.deleteEventoById(this.evento['eventoId']).subscribe(() => {
          template.hide();
          this.getEventos();
        }, error => {
          console.log(error);
        }
    );
  }
}
