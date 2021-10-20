import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { EventoService } from 'src/app/_services/evento.service';
import { Evento } from '../../../_models/Evento';

@Component({
  selector: 'app-evento-edit',
  templateUrl: './evento-edit.component.html',
  styleUrls: ['./evento-edit.component.css']
})
export class EventoEditComponent implements OnInit {

  titulo = 'Editar Evento';
  public registerForm: FormGroup;
  public evento: Evento = new Evento;
  ImagemUrl = '../../../../assets/img/77-773420_cloud-icon-png-upload-icon-font-awesome-transparent.png';
  dataEvento = Date.now();
  fileNameToUpdate: string;
  dataAtual = '';
  file: File;

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private formBuilder: FormBuilder,
    private localeService: BsLocaleService,
    private toastr: ToastrService,
    private router: ActivatedRoute
  ) {
    this.localeService.use('pt-br');
  }

  ngOnInit(): void {
    this.validation();
    this.carregarEvento();
  }

  get lotes(): FormArray{
    return <FormArray>this.registerForm.get('lotes')
  }

  get redesSociais(): FormArray{
    return <FormArray>this.registerForm.get('redesSociais')
  }

  carregarEvento(){
    const idEvento = +this.router.snapshot.paramMap.get('id');
    this.eventoService.getEventoById(idEvento)
    .subscribe((evento:Evento) => {
      this.evento = Object.assign({}, evento);
      this.fileNameToUpdate = evento.imagemUrl.toString();

      this.ImagemUrl = `http://localhost:50859/Resources/Images/${this.evento.imagemUrl}? _ts=${this.dataAtual}`;

      this.evento.imagemUrl = '';
      this.registerForm.patchValue(this.evento);

      this.evento.lotes.forEach(lote => {
        this.lotes.push(this.criaLote(lote));
      });
      this.evento.redesSociais.forEach(redeSocial =>{
        this.redesSociais.push(this.criaRedeSocial(redeSocial));
      });
    })
  }

  validation(){
    this.registerForm = this.formBuilder.group({
      id: [],
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(1200)]],
      telefone: ['', Validators.required],
      imagemUrl: [''],
      email: ['', [Validators.required, Validators.email]],
      lotes : this.formBuilder.array([]),
      redesSociais: this.formBuilder.array([]),
    });
  }

  criaLote(lote: any): FormGroup {
    return this.formBuilder.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim]
    });
  }

  criaRedeSocial(redeSocial: any): FormGroup{
    return this.formBuilder.group({
      id: [redeSocial.id],
      nome: [redeSocial.nome, Validators.required],
      url: [redeSocial.url, Validators.required]
    })
  }

  adicionarLote(){
    this.lotes.push(this.criaLote({id:0}));
  }

  adicionarRedeSocial(){
    this.redesSociais.push(this.criaRedeSocial({id:0}));
  }

  removerLote(id: number){
    this.lotes.removeAt(id);
  }

  removerRedeSocial(id: number){
    this.redesSociais.removeAt(id);
  }

  onFileChange(file: FileList){
    const reader = new FileReader();

    reader.onload = (event: any) => this.ImagemUrl = event.target.result;
    this.file = event.target.files;
    reader.readAsDataURL(file[0]);
  }

  salvarEvento(){
    this.evento = Object.assign({id: this.evento.eventoId}, this.registerForm.value);
    this.evento.imagemUrl = this.fileNameToUpdate;

    this.uploadImagem();
    this.eventoService.putEvento(this.evento).subscribe(
      () => {
        this.toastr.success('Sucesso!', 'Alterado com sucesso', {
          timeOut: 3000,
        });
      }, error => {
        this.toastr.error('Erro ao criar evento', 'Erro', {
          timeOut: 3000,
        });
      }
    );
  }

  uploadImagem(){
      if (this.registerForm.get('imagemUrl').value !== '') {
        this.eventoService.postUpload(this.file, this.fileNameToUpdate)
        .subscribe(() => {
          this.dataAtual = new Date().getMilliseconds().toString();
          this.ImagemUrl = `http://localhost:50859/Resources/Images/${this.evento.imagemUrl}? _ts=${this.dataAtual}`;
        });
      }
  }

}
