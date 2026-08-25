# Sudoku Maker

Sudoku Maker, Avalonia UI ve .NET 10 ile geliştirilmiş, Windows ve macOS için bir masaüstü Sudoku uygulaması. Her oyun, sabit bir bulmaca havuzundan çekilmek yerine anında üretiliyor, yani hiçbir iki oyun birbirinin aynısı değil. Fareyle ya da tamamen klavyeyle oynayabilir, bir hücre üzerinde düşünürken kalem notu (aday rakam) alabilir, bir bulmacayı yarıda bırakıp daha sonra kaldığın yerden devam edebilir ve çözdüklerini zorluk bazında bir liderlik tablosunda geçmiş oyunlarınla kıyaslayabilirsin.

## İçindekiler

- [Gereksinimler](#gereksinimler)
- [Projeyi çalıştırma](#projeyi-çalıştırma)
- [Özellikler](#özellikler)
- [Bulmaca üretimi ve çözümü](#bulmaca-üretimi-ve-çözümü)
- [Klavye kısayolları](#klavye-kısayolları)
- [Proje yapısı](#proje-yapısı)
- [macOS için paketleme](#macos-için-paketleme)

## Gereksinimler

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- `make` (macOS'ta varsayılan olarak kurulu gelir; Windows'ta WSL, Git Bash kullanabilir ya da `make`'i ayrıca kurabilirsin)

## Projeyi çalıştırma

Proje bir `makefile` ile geliyor, bu yüzden elle `dotnet` komutu çalıştırmana gerek yok:

```bash
make build   # projeyi derler
make start   # çalıştırır
make all     # tek adımda hem derler hem çalıştırır
```

## Özellikler

**Bulmacalar ve zorluk.** Her yeni oyun, üç zorluk seviyesinden birinde (Kolay, Orta, Zor) sıfırdan üretilir.

**Kaydetme ve devam etme.** Bir bulmacayı yarım bırakıp kaydedebilir, geçen süre, notlar ve kullanılan ipucu sayısı dahil aynı durumdan devam edebilirsin.

**Çakışma vurgulama.** Bir satır, sütun ya da 3x3 kutuda tekrar eden bir rakam varsa, o grubun tamamı anında vurgulanır — bunu görmek için Kontrol Et'e basmana gerek yok.

**Kalem notları.** Not modunu açarak bir hücrenin içine küçük bir 3x3 ızgarada aday rakamlar yazabilirsin. Bir hücreye kesin bir rakam girildiğinde notlar otomatik olarak temizlenir.

**Geri al ve ileri al.** Her rakam değişikliği takip edilir, böylece hamlelerinde ileri geri gidebilirsin.

**İpuçları.** Bir seferde bir doğru hücre gösterir. İpuçları sayılır ve puanına yansır, yani bedava değildir.

**Tamamlama.** Bir bulmacayı bitirmek süreyi durdurur, oyunu otomatik olarak kaydeder ve puanınla birlikte bir kereliğine bir özet gösterir. Zaten bitmiş bir kaydı tekrar açmak bu özeti bir daha göstermez, süre de bitirdiğin anda donmuş kalır.

**Puanlama ve liderlik tablosu.** Puanın; zorluğa, bitirme süresine, kullanılan ipucu sayısına ve rakamları ne kadar istikrarlı girdiğine bağlıdır. Ana menüde, Kolay, Orta ve Zor için ayrı sıralamaların olduğu, puana ya da süreye göre sıralanabilen bir liderlik tablosu var.

**PDF'e aktarma.** Mevcut bulmacayı yazdırılabilir bir PDF olarak dışa aktarabilirsin.

**Dil.** Arayüzün tamamı İngilizce ve Türkçe olarak mevcut, ana menüden yeniden başlatmaya gerek kalmadan anında değiştirilebilir.

## Bulmaca üretimi ve çözümü

Hem üretim hem çözüm, aynı temel yönteme dayanır: ızgarayı hücre hücre gezen, bir rakam deneyen ve özyinelemeli olarak devam eden bir **geri izleme (backtracking) araması**. Bir çıkmaza girilirse, o rakam geri alınır ve bir sonraki denenir.

Baştan çözülmüş, dolu bir tahta oluşturmak için üretici bu geri izleme aramasını boş bir ızgara üzerinde çalıştırır, ama her hücrede `1`–`9` arasındaki rakamları deneme sırasını karıştırır. Her seferinde farklı bir çözüme ulaşılmasını sağlayan şey tam olarak bu — aksi halde her seferinde aynı dolu tahta üretilirdi.

Çözülmüş bir tahtayı bulmacaya dönüştürmek için üretici hücreleri rastgele sırayla seçip birer birer boşaltır. Bir hücreyi boşalttıktan sonra tahtayı tekrar çözerken kaç farklı çözüm olduğunu sayar, ikinci bir çözüm bulur bulmaz aramayı durdurur. Bulmaca hâlâ tam olarak tek bir çözüme sahipse hücre boş kalır; boşaltma bulmacayı belirsiz hale getirdiyse rakam geri konur. Bu tekillik kontrolü, kaç hücre boş kalırsa kalsın, üretilen her bulmacanın tam olarak tek bir geçerli çözümü olmasını garanti eder. Zorluk seviyeleri ise sadece kaç hücrenin boşaltılacağı hedefini belirler — boş hücre ne kadar fazlaysa, kendi başına çözmen gereken kısım da o kadar fazladır.

## Klavye kısayolları

| Tuş | Etki |
|---|---|
| `1`–`9` (üst sıra ya da numpad) | Seçili hücreye rakam gir, Not modu açıksa aday rakamı işaretle |
| Ok tuşları | Seçili hücreyi taşı |
| `Delete` / `Backspace` | Seçili hücreyi temizle (değer ve notlar) |
| `N` | Not modunu aç/kapat |
| `Ctrl+Z` | Geri al |
| `Ctrl+Y` ya da `Ctrl+Shift+Z` | İleri al |

## Proje yapısı
```
sudoku-maker/
├── Models/ Difficulty, SaveGame, SudokuBoard, ...
├── ViewModels/ SudokuViewModel, SudokuCellViewModel, LeaderboardViewModel, ...
├── Views/ MainWindow, SudokuView, LeaderboardView, dialoglar, ...
├── Services/ SudokuGenerator, SudokuSolver, SaveGameService, PdfExportService, LocalizationService
├── Localization/ Anlık dil değiştirme için kullanılan {loc:Loc} markup extension'ı
└── Assets/ İkonlar ve SVG buton görselleri
```