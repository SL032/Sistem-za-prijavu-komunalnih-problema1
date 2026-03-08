# Sistem-za-prijavu-komunalnih-problema

<p align="center">
 <img src="image.png" width="800"/>
</p>

### Funkcionalni zahtevi – Građanin (Korisnik)

| ID  | Zahtev                         | Opis                                                                                                  | Prioritet |
|:---:|--------------------------------|-------------------------------------------------------------------------------------------------------|:---------:|
| 1  | Registracija naloga            | Sistem omogućava kreiranje naloga uz validaciju email-a i obaveznih polja.                          | 1 |
| 2  | Prijava (Login)                | Sistem omogućava prijavu korisnika i proveru kredencijala.                                          | 1 |
| 3  | Kreiranje prijave              | Korisnik kreira **Prijavu** izborom **Komunalnog Problema**, unosom opisa i dodelom **Lokacije**.  | 1 |
| 4  | Dodavanje lokacije             | Korisnik unosi adresu i opcioni opis lokacije; sistem čuva kao **Lokacija**.                        | 1 |
| 5  | Upload dokumenata              | Korisnik dodaje jedan ili više **Dokumenata** (slika/PDF) na prijavu.                               | 1 |
| 6  | Pregled mojih prijava          | Korisnik vidi listu svojih prijava sa datumom, statusom i lokacijom.                                | 1 |
| 7  | Detalji prijave                | Korisnik otvara detalje prijave i vidi sve podatke i dokumente.                                     | 1 |
| 8  | Izmena prijave (dok je nova)   | Izmena je dozvoljena dok je status **„Novo“**.                                                       | 2 |
| 9  | Brisanje prijave (dok je nova) | Brisanje je dozvoljeno dok je status **„Novo“**.                                                     | 2 |
| 10 | Pretraga i filtriranje         | Pretraga po statusu, datumu, tipu problema i adresi.                                                 | 2 |
| 11 | Validacija unosa               | Validacija dužine opisa, email formata i tipa dokumenta.                                             | 1 |

---

### Funkcionalni zahtevi – Operater komunalne službe

| ID | Zahtev                    | Opis                                                                                              | Prioritet |
|:--:|---------------------------|---------------------------------------------------------------------------------------------------|:---------:|
| 1 | Prijava operatera         | Operater se prijavljuje i dobija pristup prema ulozi/službi.                                     | 1 |
| 2 | Pregled prijava za službu | Pregled prijava dodeljenih njegovoj **Komunalnoj Službi**.                                       | 1 |
| 3 | Promena statusa prijave   | Promena statusa (Novo → U obradi → Rešeno / Odbijeno).                                           | 1 |
| 4 | Dodela/izmena službe      | Promena **SlužbaID** ako je prijava pogrešno usmerena.                                           | 2 |
| 5 | Pregled dokumenata        | Pregled svih dokumenata vezanih za prijavu.                                                       | 1 |
| 6 | Dodavanje dokumenata      | Dodavanje terenskih fotografija ili zapisnika.                                                   | 2 |
| 7 | Filtriranje i sortiranje  | Filtriranje po statusu, datumu, tipu problema i lokaciji.                                        | 2 |
| 8 | Evidencija obrade         | Ažuriranje opisa ili napomene prijave.                                                            | 2 |
