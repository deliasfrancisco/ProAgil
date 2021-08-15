import { Lote } from "./Lote";
import { Palestrante } from "./Palestrante";
import { RedeSocial } from "./RedeSocial";

export interface Evento {
  EventoId:number;
  Local:string;
  DataEvento:Date;
  Tema:string;
  QtdPessoas:number
  ImagemUrl:string;
  Telefone:string;
  Email:string;
  Lotes:Lote[];
  RedesSociais:RedeSocial[];
  PalestrantesEventos:Palestrante[];
}
