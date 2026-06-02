

String codificacion();

String codificacion(){
    String qr = "";
    valor = random(000000, 999999);
    qr = id_aula   + "-" + valor;
    String stringvalor = String(valor);
    Post(id_aula, stringvalor);
    return qr;
}
