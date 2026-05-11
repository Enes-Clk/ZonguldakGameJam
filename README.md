# ZonguldakGameJam - Core Loop ve Dukkan Kurulum Rehberi

Bu dokuman, yeni oyun dongusunu (core loop), ekonomi/upgrade sistemini ve dukkani sifirdan kurman icin adim adim bir yol haritasi verir. En alttaki kontrol listesi ile her seyi tek tek tamamlayabilirsin.

---

## 1) Genel Mimarinin Ozeti

- `PersistentManager`: Para, balık sayisi ve gun gibi kalici verileri tutar. Kalici statlarin **Base** degerleri buradadir.
- `DayManager`: Gunluk gecici buff'lar. Sahne yeniden yuklenince sifirlanir.
- `TownManager`: 4 farkli bolgeyi yonetir (Balik satisi, Kalici upgrade, Gecici upgrade, Ev).
- `ShopUIManager`: Dukkan butonlarini ve uyarilari yonetir.
- `HookController`: Kanca sure limiti ve kapasite dolunca otomatik geri sarma yapar.

---

## 2) Dosyalar (Eklenecek / Guncellenecek)

**Eklenecek:**
- `Assets/Scripts/PersistentManager.cs`
- `Assets/Scripts/DayManager.cs`
- `Assets/Scripts/TownManager.cs`
- `Assets/Scripts/ShopUIManager.cs`

**Guncellenecek:**
- `Assets/Scripts/Ship/HookController.cs`
- `Assets/Scripts/Fish.cs`
- `Assets/Scripts/FishingManager.cs`

---

## 3) Sahne Kurulumu (Adim Adim)

### A) PersistentManager (Kalici Veri)
1. İlk sahnede bos bir GameObject olustur (ornek isim: `PersistentManagerObj`).
2. `PersistentManager` scriptini ekle.
3. Bu obje sahneler arasi kalici olacak (DontDestroyOnLoad).

### B) DayManager (Gunluk Buff)
1. Her gun sahnesine bos bir GameObject olustur (ornek isim: `DayManagerObj`).
2. `DayManager` scriptini ekle.
3. Bu obje **DontDestroyOnLoad** DEGIL.

### C) Town (Sehir Binasi Tek Sprite + 4 Trigger)
Sehir binasi **tek sprite**, altinda **4 ayrik BoxCollider2D** var:

- **Sell (Balik Satis)**
- **Upgrade (Kalici Upgrade)**
- **Temp Upgrade (Gecici Buff)**
- **Home (Gun Bitirme / Ev)**

**Kurulum:**
1. Sehir binasi GameObject’ine `TownManager` ekle.
2. Altina 4 adet child GameObject ekle, her birine `BoxCollider2D` ekle, `IsTrigger = true` yap.
3. Bu 4 collider’i `TownManager` alanlarina ata:
   - `sellTrigger`, `upgradeTrigger`, `tempUpgradeTrigger`, `homeTrigger`
4. Player objesinin Tag’i **Player** olmali.

### D) UI Promptlar ve Paneller
Her bolge icin 1 prompt ("E’ye bas") ve 1 panel:

- Sell Prompt + Sell Panel (tek "Sat" butonu)
- Upgrade Prompt + Upgrade Panel (Kalici upgrade)
- Temp Prompt + Temp Panel (Gecici buff)
- Home Prompt + Home Panel (istege bagli)

`TownManager` alanlarini doldur:
- `sellPrompt`, `upgradePrompt`, `tempUpgradePrompt`, `homePrompt`
- `sellPanel`, `upgradePanel`, `tempUpgradePanel`, `homePanel`

**Not:** Paneller baslangicta `SetActive(false)` olmali.

### E) Ev / Gun Bitirme UI
`TownManager` icin:
- `fadeGroup` (CanvasGroup) ekle ve ata
- `dayText` (TMP) ekle ve ata

Gun bitince ekran karariyor, "Gun: X" gosteriliyor, sonra sahne reload oluyor.

---

## 4) Balik Satis Sistemi

Balik satisinda:
- `fishCount * 10` kadar para eklenir.
- `fishCount` sifirlanir.

Bu islem `TownManager` icinde otomatik calisir.

---

## 5) Upgrade Sistemleri

### A) Kalici Upgrade (Permanent)
Kalici olarak **Base Value** artar:
- `baseMaxInventory`
- `baseHookDistance`
- `baseHookDamage`
- `baseHookSpeed`

Bu degerler `PersistentManager` icinde tutulur.

### B) Gecici Buff (Temp Upgrade)
Gunluk carpani **DayManager** icinde tutulur:
- `damageMultiplier`
- `distanceMultiplier`
- `inventoryMultiplier`
- `speedBuff` (hiz carpani DEGIL, toplanan deger)

---

## 6) Dukkan Butonlarini Baglama

`ShopUIManager` scriptini upgrade ve temp paneline ekle.

### Kalici Upgrade Butonlari:
- `BuyDamageUpgrade()`
- `BuyDistanceUpgrade()`
- `BuySpeedUpgrade()`
- `BuyInventoryUpgrade()`

### Gecici Buff Butonlari:
- `BuyDamageBuff()`
- `BuyDistanceBuff()`
- `BuySpeedBuff()`
- `BuyInventoryBuff()`

### Uyari Sistemi
`warningText` alanina bir TMP Text ata. Para yetmezse 2 saniye "Yetersiz Bakiye!" yazilir.

---

## 7) Kanca Limitleri

`HookController` icinde:
- `hookTimeLimit` dolunca kanca geri sarar.
- Envanter dolu ise kanca geri sarar.

Envanter limiti:
- `DayManager.GetFinalMaxInventory()` ile hesaplanir.

---

## 8) Stats Hesabi (Kritik Kural)

- **Hasar / Mesafe / Envanter:**
  - `Final = PersistentManager.Base * DayManager.Multiplier`

- **Hiz (ISTISNA):**
  - `Final Speed = PersistentManager.baseHookSpeed + DayManager.speedBuff`

---

## 9) Kontrol Listesi (Yapilacaklar)

- [ ] `PersistentManager` sahnede ve DontDestroyOnLoad
- [ ] `DayManager` her gun sahnesinde
- [ ] Sehir binasi altinda 4 trigger var (Sell/Upgrade/Temp/Home)
- [ ] `TownManager` tum trigger/panel/prompt alanlari dolu
- [ ] Player Tag = `Player`
- [ ] Sell panelinde sadece "Sat" butonu var
- [ ] Upgrade panelinde kalici butonlar var
- [ ] Temp panelinde buff butonlari var
- [ ] `ShopUIManager` butonlara bagli
- [ ] `warningText` atanmis
- [ ] `HookController.hookTimeLimit` ayarlandi

---

## 10) Hata Ayiklama Ipuclari

- Panel acilmiyorsa: Trigger collider `IsTrigger = true` mi?
- E’ye basinca olmuyorsa: Player Tag `Player` mi?
- Para artmiyorsa: `PersistentManager` sahnede mi?
- Upgrade calismiyorsa: `ShopUIManager` butonlara bagli mi?

---

Bu rehberi adim adim uygularsan tum sistem sorunsuz calisir. Takildigin noktada ekran goruntusu veya hata mesaji paylas, beraber cozebiliriz.
