using Api.Dtos;
using Api.Models;
using Api.ServiceRol;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Api.Controllers.Api
{
    /// <summary>
    /// Raccomandata online
    /// </summary>
    [RoutePrefix("api/Rol")]
    public class RolController : ApiController
    {

        private static Entities _context;

        public RolController()
        {
            _context = new Entities();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [Route("NewService")]
        [HttpGet]
        public string getNewService(Guid guidUser)
        {
            var r = getRequestId(guidUser);
            return r;
        }

        [Route("ServiceRol")]
        [HttpPost]
        public static ROLServiceSoapClient getNewServiceRol(Guid guidUser)
        {
            //MULTIPLE USERS
            var u = _context.Users.Where(a => a.guidUser == guidUser);
            if (u.Count() == 0)
                return null;

            var Users = _context.Users.Where(a => a.guidUser == guidUser).SingleOrDefault(a => a.parentId == 0);
            if (Users == null)
                return null;
            ROLServiceSoapClient service = new ROLServiceSoapClient();
            if (Users.areaTestUser)
            {
                service.ClientCredentials.UserName.UserName = Users.usernamePosteAreaTest;
                service.ClientCredentials.UserName.Password = Users.pwdPosteAreaTest;
            }
            else
            {
                service.ClientCredentials.UserName.UserName = Users.usernamePoste;
                service.ClientCredentials.UserName.Password = Users.pwdPoste;
            }
            return service;
        }


        [Route("UPU")]
        [HttpGet]
        public static string UPU(Guid guidUser)
        {
            //MULTIPLE USERS
            var u = _context.Users.Where(a => a.guidUser == guidUser);
            if (u.Count() == 0)
                return null;

            var Users = _context.Users.Where(a => a.guidUser == guidUser).SingleOrDefault(a => a.parentId == 0);
            if (Users == null)
                return null;
            ROLServiceSoapClient service = new ROLServiceSoapClient();
            if (Users.areaTestUser)
            {
                service.ClientCredentials.UserName.UserName = Users.usernamePosteAreaTest;
                service.ClientCredentials.UserName.Password = Users.pwdPosteAreaTest;
            }
            else
            {
                service.ClientCredentials.UserName.UserName = Users.usernamePoste;
                service.ClientCredentials.UserName.Password = Users.pwdPoste;
            }
            var n = service.RecuperaNomiNazioniUPU(CountryLanguage.Italiano);
            var l = n.ListaNomi;
            return JsonConvert.SerializeObject(l);
        }

        private static string getRequestId(Guid guid)
        {
            ROLServiceSoapClient service = new ROLServiceSoapClient();
            service = getNewServiceRol(guid);
            var IdRichiesta = service.RecuperaIdRichiesta().IDRichiesta;
            service.Close();
            return IdRichiesta;
        }

        private static Destinatario GetDestinatarioRol(NamesDto name, int? index = 1)
        {
            Destinatario d = new Destinatario();
            Nominativo n = new Nominativo();
            Indirizzo i = new Indirizzo();
            n.RagioneSociale = name.businessName;
            n.Cognome = name.surname;
            n.Nome = name.name;
            n.ComplementoNominativo = name.complementNames;
            n.CAP = name.cap;
            n.Citta = name.city;
            n.Provincia = name.province;
            n.Stato = name.state;
            n.ComplementoIndirizzo = name.complementAddress;
            n.CodiceFiscale = name.fiscalCode;

            i.DUG = name.dug;
            i.Toponimo = name.address;
            i.NumeroCivico = name.houseNumber;
            i.Esponente = string.Empty;
            n.Indirizzo = i;

            n.Telefono = string.Empty;
            n.TipoIndirizzo = ServiceRol.NominativoTipoIndirizzo.NORMALE;
            n.UfficioPostale = "";
            n.Zona = string.Empty;
            n.CasellaPostale = "";
            n.Frazione = string.Empty;

            d.IdDestinatario = Convert.ToString(index);
            d.IdRicevuta = String.Empty;
            d.Nominativo = n;

            return d;
        }

        private static OpzionidiStampa GetOpzioniDiStampa(tipoStampa tipoStampa, fronteRetro fronteRetro)
        {
            var opzioniDiStampa = new OpzionidiStampa();
            opzioniDiStampa.BW = (tipoStampa == tipoStampa.colori ? "true" : "false");
            opzioniDiStampa.FronteRetro = (fronteRetro == fronteRetro.fronte ? "true" : "false");
            return opzioniDiStampa;
        }

        private static ROLSubmitOpzioni GetOpzioniRol(tipoStampa tipoStampa, fronteRetro fronteRetro)
        {

            var Opzioni = new ROLSubmitOpzioni();
            Opzioni.Archiviazione = false;
            Opzioni.DataStampa = DateTime.Now;
            Opzioni.DPM = false;
            Opzioni.OpzionidiStampa = GetOpzioniDiStampa(tipoStampa, fronteRetro);
            Opzioni.FirmaElettronica = false;
            Opzioni.InserisciMittente = false;
            Opzioni.Inserti = new ROLSubmitOpzioniInserti();
            Opzioni.Inserti.InserisciMittente = false;
            Opzioni.Inserti.Inserto = string.Empty;

            return Opzioni;
        }

        private static Mittente GetMittente(SenderDto sender)
        {

            Mittente Mittente = new Mittente();
            Nominativo Nominativo = new Nominativo();
            Indirizzo Indirizzo = new Indirizzo();

            Nominativo.CAP = sender.cap;
            Nominativo.Citta = sender.city;
            Nominativo.Cognome = sender.surname;
            Nominativo.Nome = sender.name;
            Nominativo.Provincia = sender.province;
            Nominativo.Stato = sender.state;
            Nominativo.RagioneSociale = sender.businessName;

            Indirizzo.DUG = sender.dug;
            Indirizzo.NumeroCivico = sender.houseNumber;
            Indirizzo.Toponimo = sender.address;
            Indirizzo.Esponente = string.Empty;

            Nominativo.Telefono = string.Empty;
            Nominativo.TipoIndirizzo = NominativoTipoIndirizzo.NORMALE;
            Nominativo.UfficioPostale = "";
            Nominativo.Zona = string.Empty;
            Nominativo.ComplementoIndirizzo = string.Empty;
            Nominativo.CasellaPostale = "";
            Nominativo.ComplementoNominativo = string.Empty;
            Nominativo.Frazione = string.Empty;
            Nominativo.Indirizzo = Indirizzo;

            Mittente.Nominativo = Nominativo;
            Mittente.InviaStampa = false;

            return Mittente;
        }

        public static Documento[] getDoc(List<string> strNomeFile, int NumeroDiDocumenti)
        {
            System.IO.FileInfo file;
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            Documento[] ArrayDocumento = new Documento[NumeroDiDocumenti - 1 + 1];
            for (var i = 0; i <= NumeroDiDocumenti - 1; i++)
            {
                Documento documento = new Documento();
                file = new System.IO.FileInfo(strNomeFile[i]);
                documento.TipoDocumento = "pdf";
                documento.Immagine = System.IO.File.ReadAllBytes(strNomeFile[i]);
                documento.MD5 = System.BitConverter.ToString(md5.ComputeHash(documento.Immagine)).Replace("-", string.Empty);
                ArrayDocumento[i] = documento;
            }
            return ArrayDocumento;
        }

        public static Bollettino896 getBollettino896(BulletinsDtos bollettino)
        {
            Bollettino896 b = new Bollettino896();
            b.NumeroContoCorrente = bollettino.numeroContoCorrente;
            b.IntestatoA = bollettino.intestatoA;
            b.FormatoStampa = 0;
            b.AdditionalInfo = "";
            b.IBAN = "";
            b.EseguitoDa = new BollettinoEseguitoDa();
            b.EseguitoDa.Nominativo = bollettino.eseguitoDaNominativo;
            b.EseguitoDa.Indirizzo = bollettino.eseguitoDaIndirizzo;
            b.EseguitoDa.CAP = bollettino.eseguitoDaCAP;
            b.EseguitoDa.Localita = bollettino.eseguitoDaLocalita;
            b.CodiceCliente = bollettino.codiceCliente;
            b.ImportoEuro = bollettino.importoEuro;
            b.Causale = bollettino.causale;
            return b;
        }

        public static Bollettino674 getBollettino674(BulletinsDtos bollettino)
        {
            Bollettino674 b = new Bollettino674();
            b.NumeroContoCorrente = bollettino.numeroContoCorrente;
            b.IntestatoA = bollettino.intestatoA;
            b.FormatoStampa = 0;
            b.AdditionalInfo = "";
            b.IBAN = "";
            b.EseguitoDa = new BollettinoEseguitoDa();
            b.EseguitoDa.Nominativo = bollettino.eseguitoDaNominativo;
            b.EseguitoDa.Indirizzo = bollettino.eseguitoDaIndirizzo;
            b.EseguitoDa.CAP = bollettino.eseguitoDaCAP;
            b.EseguitoDa.Localita = bollettino.eseguitoDaLocalita;
            b.CodiceCliente = bollettino.codiceCliente;
            b.Causale = bollettino.causale;
            return b;
        }

        public static Bollettino451 getBollettino451(BulletinsDtos bollettino)
        {
            Bollettino451 b = new Bollettino451();
            b.NumeroContoCorrente = bollettino.numeroContoCorrente;
            b.IntestatoA = bollettino.intestatoA;
            b.FormatoStampa = 0;
            b.AdditionalInfo = "";
            b.IBAN = "";
            b.EseguitoDa = new BollettinoEseguitoDa();
            b.EseguitoDa.Nominativo = bollettino.eseguitoDaNominativo;
            b.EseguitoDa.Indirizzo = bollettino.eseguitoDaIndirizzo;
            b.EseguitoDa.CAP = bollettino.eseguitoDaCAP;
            b.EseguitoDa.Localita = bollettino.eseguitoDaLocalita;
            b.ImportoEuro = bollettino.importoEuro;
            b.Causale = bollettino.causale;
            return b;
        }

        [Route("SendNames")]
        [HttpGet]
        public async Task<Names> sendNames(GetRecipent GetRecipent, int operationId, SenderDto sender, int userId)
        {
            var n = _context.Names.SingleOrDefault(a => a.id == GetRecipent.recipient.id);

            var user = _context.Users.SingleOrDefault(a => a.id == userId);
            var guidUser = user.guidUser;

            //PREVERIFICA BOLLETTINO
            if (GetRecipent.bulletin != null)
            {
                ControlloBollettino ctrlB = GlobalClass.verificaBollettino(GetRecipent.bulletin);
                if (!ctrlB.Valido)
                {
                    n.valid = false;
                    n.stato = "Errore nella convalida del bollettino.";
                    _context.SaveChanges();
                    return null;
                }
            }

            ROLServiceSoapClient service = new ROLServiceSoapClient();
            service = getNewServiceRol(guidUser);

            var fileName = GetRecipent.recipient.fileName;
            List<string> filesName = new List<string>();
            filesName.Add(fileName);

            var requestId = getRequestId(guidUser);


            tipoStampa ts = tipoStampa.colori;
            if (GetRecipent.recipient.tipoStampa)
                ts = tipoStampa.biancoNero;

            fronteRetro fr = fronteRetro.fronte;
            if (GetRecipent.recipient.fronteRetro)
                fr = fronteRetro.fronteRetro;

            Destinatario d = new Destinatario();
            d = GetDestinatarioRol(GetRecipent.recipient);
            Destinatario[] ld = new Destinatario[1];
            ld[0] = d;
            ROLSubmit rs = new ROLSubmit();
            rs.Destinatari = ld;
            rs.Opzioni = GetOpzioniRol(ts, fr);
            rs.Mittente = GetMittente(sender);
            rs.NumeroDestinatari = 1;
            rs.Documento = getDoc(filesName, 1);

            //BOLLETTINO
            if (GetRecipent.bulletin != null)
            {
                PaginaBollettino pagina = new PaginaBollettino();
                object b = null;
                switch (GetRecipent.bulletin.bulletinType)
                {
                    case (int)bulletinType.Bollettino451:
                        b = getBollettino451(GetRecipent.bulletin);
                        pagina.Bollettino = (Bollettino451)b;
                        break;
                    case (int)bulletinType.Bollettino674:
                        b = getBollettino674(GetRecipent.bulletin);
                        pagina.Bollettino = (Bollettino674)b;
                        break;
                    case (int)bulletinType.Bollettino896:
                        b = getBollettino896(GetRecipent.bulletin);
                        pagina.Bollettino = (Bollettino896)b;
                        break;
                }

                PaginaBollettinoBase[] p = new PaginaBollettinoBase[1];
                p[0] = pagina;
                rs.PagineBollettini = p;
            }

            //RICEVUTA DI RITORNO
            if (GetRecipent.recipient.ricevutaRitorno)
            {
                DatiRicevuta dr = new DatiRicevuta();
                dr.Nominativo = rs.Mittente.Nominativo;
                rs.DatiRicevuta = dr;
            }

            rs.Nazionale = (GetRecipent.recipient.state.ToUpper() == "ITALIA" ? "true" : "false");

            InvioResult esito = new InvioResult();
            try
            {
                esito = service.Invio(requestId, user.businessName, rs);

                if (esito.CEResult.Type != "I")
                {
                    n.valid = false;
                    n.stato = esito.CEResult.Description;
                    _context.SaveChanges();
                    return n;
                }
                n.requestId = esito.IDRichiesta;
                n.guidUser = esito.GuidUtente;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                n.valid = false;
                n.stato = "Errore nella richiesta del submit.";
                _context.SaveChanges();
                return n;
            };

            return n;
        }

        [Route("ValorizzaConfirm")]
        [HttpGet]
        public async Task<bool> ValorizzaConfirm(List<Names> names, Guid guidUser)
        {
            ROLServiceSoapClient service = new ROLServiceSoapClient();
            service = getNewServiceRol(guidUser);

            Richiesta[] Richiesta = new Richiesta[names.Count];
            int y = 0;
            foreach (var n in names)
            {
                Richiesta r = new Richiesta();
                r.GuidUtente = n.guidUser;
                r.IDRichiesta = n.requestId;
                Richiesta[y] = r;
                y++;
            }

            Thread.Sleep(15000);
            var v = await service.ValorizzaAsync(Richiesta);

            int i = 0;
            if (v.ValorizzaResult.CEResult.Type != "I")
                do
                {
                    Thread.Sleep(5000);
                    v = await service.ValorizzaAsync(Richiesta);
                    i++;
                } while (v.ValorizzaResult.CEResult.Type != "I" && i < 50);

            if (v.ValorizzaResult.CEResult.Type != "I")
            {
                foreach (var n in names)
                {
                    var nn = _context.Names.SingleOrDefault(a => a.requestId == n.requestId);
                    nn.requestId = null;
                    _context.SaveChanges();
                }
                return false;
            }

            var ServizioEnquiryResponse = v.ValorizzaResult.ServizioEnquiryResponse.ToList();

            var conferma = await service.PreConfermaAsync(Richiesta, true);

            int z = 0;
            foreach (var s in ServizioEnquiryResponse)
            {
                var n = _context.Names.SingleOrDefault(a => a.requestId == s.Richiesta.IDRichiesta);
                if (s.StatoLavorazione.Descrizione.ToLower() == "prezzato")
                {
                    var p = new Prices()
                    {
                        price = Convert.ToDecimal(s.Totale.ImportoNetto),
                        vatPrice = Convert.ToDecimal(s.Totale.ImportoIva),
                        totalPrice = Convert.ToDecimal(s.Totale.ImportoTotale)
                    };

                    if (conferma.PreConfermaResult.DestinatariRaccomandata[z] != null)
                    {
                        n.guidUser = s.Richiesta.GuidUtente;
                        n.orderId = conferma.PreConfermaResult.IdOrdine;
                        n.price = Convert.ToDecimal(s.Totale.ImportoNetto);
                        n.vatPrice = Convert.ToDecimal(s.Totale.ImportoIva);
                        n.totalPrice = Convert.ToDecimal(s.Totale.ImportoTotale);
                        n.currentState = (int)currentState.PresoInCarico;
                        n.codice = conferma.PreConfermaResult.DestinatariRaccomandata[z].NumeroRaccomandata;
                    }
                    else
                    {
                        n.valid = false;
                        n.stato = "Errore nella conferma dell'invio";
                    }
                }
                else
                {
                    n.valid = false;
                    n.stato = "Errore nella conferma dell'invio";
                }
                _context.SaveChanges();
                z++;

            }

            return true;

        }

    }
}
