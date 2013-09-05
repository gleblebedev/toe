using System.Text;

using NUnit.Framework;

namespace Toe.CircularArrayQueue.Tests
{
	[TestFixture]
	public class ExtensionMethods
	{
		#region Public Methods and Operators

		[Test]
		public void TestFloat()
		{
			using (var b = new ThreadSafeWriteQueue(10))
			{
				var pos = b.Allocate(9);

				this.TestFloat(b, pos, float.MinValue);
				this.TestFloat(b, pos, float.MaxValue);
				this.TestFloat(b, pos, float.Epsilon);
				this.TestFloat(b, pos, 0);
				this.TestFloat(b, pos, 1.1f);
			}
		}

		[Test]
		public void TestInt64()
		{
			using (var b = new ThreadSafeWriteQueue(10))
			{
				var pos = b.Allocate(9);

				this.TestInt64(b, pos, long.MinValue);
				this.TestInt64(b, pos, long.MaxValue);
				this.TestInt64(b, pos, int.MaxValue);
				this.TestInt64(b, pos, int.MinValue);
				this.TestInt64(b, pos, 0);
			}
		}

		[Test]
		public void TestString()
		{
			using (var b = new ThreadUnsafeQueue(1024))
			{
				for (int i = 1; i < char.MaxValue; i++)
				{
					var c = ((char)i).ToString();
					var testTest = c + c + c;
					var bytes = Encoding.UTF8.GetBytes(testTest);
					var len = b.GetStringLength(testTest);
					Assert.AreEqual(len, (bytes.Length + 4) >> 2, string.Format("Wrong size of char {0} ({1})", i, c));
				}
			}
		}

        [Test]
        public void TestUnicode()
        {
            string[] testValues = new string[]
                {
                    "العربية "," Български "," Català "," Česky "," Dansk "," Eesti "," Esperanto "," Euskara "," فارسی "," Galego "," 한국어 "," हिन्दी "," Hrvatski "," Bahasa Indonesia "," עברית "," Lietuvių "," Magyar "," Bahasa Melayu "," 日本語 "," Norsk (bokmål "," nynorsk) "," Oʻzbekcha "," Polski "," Português "," Қазақша / Qazaqşa / قازاقشا "," Română "," Simple English "," Sinugboanon "," Slovenčina "," Slovenščina "," Српски / Srpski "," Suomi "," Türkçe "," Українська "," Tiếng Việt "," Volapük "," Winaray "," 中文",
                    "Afrikaans "," Alemannisch "," አማርኛ "," Aragonés "," Asturianu "," Kreyòl Ayisyen "," Azərbaycan / آذربايجان ديلی "," বাংলা "," Bân-lâm-gú "," Basa Banyumasan "," Башҡортса "," Беларуская (Акадэмічная "," Тарашкевiца) "," বিষ্ণুপ্রিয়া মণিপুরী "," Bosanski "," Brezhoneg "," Чӑвашла "," Cymraeg "," Ελληνικά "," Fiji Hindi "," Frysk "," Gaeilge "," Gàidhlig "," ગુજરાતી "," Հայերեն "," Ido "," Interlingua "," Íslenska "," Basa Jawa "," ಕನ್ನಡ "," ქართული "," Kurdî / كوردی "," Кыргызча "," Latina "," Latviešu "," Lëtzebuergesch "," Lumbaart "," Македонски "," Malagasy "," മലയാളം "," मराठी "," مصرى "," مازِرونی / Mäzeruni "," မြန်မာဘာသာ "," नेपाल भाषा "," नेपाली "," Nnapulitano "," Occitan "," Piemontèis "," Plattdüütsch "," Runa Simi "," شاہ مکھی پنجابی "," Scots "," Shqip "," Sicilianu "," Srpskohrvatski / Српскохрватски "," Soranî / کوردی "," Basa Sunda "," Kiswahili "," Tagalog "," தமிழ் "," Татарча / Tatarça "," తెలుగు "," Тоҷикӣ "," ไทย "," ᨅᨔ ᨕᨙᨁᨗ / Basa Ugi "," اردو "," Vèneto "," Walon "," ייִדיש "," Yorùbá "," 粵語 "," Žemaitėška",
                    "Bahsa Acèh "," Адыгэбзэ "," Arpitan "," ܐܬܘܪܝܐ "," Avañe’ẽ "," Авар "," Aymar "," Bahasa Banjar "," भोजपुरी "," Bikol Central "," Boarisch "," བོད་ཡིག "," Chavacano de Zamboanga "," Corsu "," Deitsch "," ދިވެހި "," Diné Bizaad "," Dolnoserbski "," Eald Englisc "," Emigliàn–Rumagnòl "," Эрзянь "," Estremeñu "," Føroyskt "," Furlan "," Gaelg "," Gagauz "," 贛語 "," گیلکی "," Hak-kâ-fa / 客家話 "," Хальмг "," ʻŌlelo Hawaiʻi "," Hornjoserbsce "," Ilokano "," Interlingue "," Иронау "," Kalaallisut "," Kapampangan "," Kaszëbsczi "," Kernewek / Karnuack "," ភាសាខ្មែរ "," Kinyarwanda "," Коми "," Кырык Мары "," Dzhudezmo / לאדינו "," Лакку "," Лезги "," Líguru "," Limburgs "," Lingála "," lojban "," Malti "," 文言 "," Māori "," მარგალური "," Mirandés "," Мокшень "," Монгол "," Nāhuatl "," Nedersaksisch "," Nordfriisk "," Nouormand / Normaund "," Novial "," Нохчийн "," Олык Марий "," ଓଡି଼ଆ "," অসমীযা় "," पाऴि "," Pangasinán "," ਪੰਜਾਬੀ / پنجابی "," Papiamentu "," پښتو "," Перем Коми "," Pfälzisch "," Picard "," Къарачай–Малкъар "," Qırımtatarca "," Ripoarisch "," Rumantsch "," Русиньскый Язык "," संस्कृतम् "," Sámegiella "," Sardu "," Saxa Tyla / Саха Тыла "," Seeltersk "," ChiShona "," සිංහල "," Ślůnski "," Soomaaliga "," Sranantongo "," Taqbaylit "," Tarandíne "," Tok Pisin "," faka Tonga "," تركمن  / Туркмен "," Удмурт "," Uyghur / ئۇيغۇرچه "," Võro "," Vepsän "," West-Vlams "," Wolof "," 吴语 "," Zazaki "," Zeêuws",
                    "Akan "," Аҧсуа "," Armãneashce "," Bamanankan "," Bislama "," Буряад "," Chamoru "," Chi-Chewa "," Cuengh "," Eʋegbe "," Fulfulde "," Gĩkũyũ "," 𐌲𐌿𐍄𐌹𐍃𐌺 "," Hausa / هَوُسَا "," Igbo "," ᐃᓄᒃᑎᑐᑦ / Inuktitut "," Iñupiak "," कश्मीरी / كشميري "," Kongo "," ພາສາລາວ "," Latgaļu "," Luganda "," Reo Mā`ohi "," Mìng-dĕ̤ng-ngṳ̄ "," Молдовеняскэ "," Na Vosa Vakaviti "," dorerin Naoero "," Nēhiyawēwin / ᓀᐦᐃᔭᐍᐏᐣ "," Norfuk / Pitkern "," Afaan Oromoo "," Ποντιακά "," Qaraqalpaqsha "," རྫོང་ཁ "," Romani / रोमानी "," Kirundi "," Gagana Sāmoa "," Sängö "," Sesotho "," Sesotho sa Leboa "," Setswana "," سنڌي / सिन्ध "," Словѣ́ньскъ / ⰔⰎⰑⰂⰡⰐⰠⰔⰍⰟ "," SiSwati "," Tetun "," ትግርኛ "," ᏣᎳᎩ "," chiTumbuka "," Xitsonga "," Tsėhesenėstsestotse "," Tshivenḓa "," Twi "," isiXhosa "," isiZulu",
                    "Other languages "," Weitere Sprachen "," Autres langues "," Kompletna lista języków "," 他の言語 "," Otros idiomas "," 其他語言 "," Другие языки "," Aliaj lingvoj "," 다른 언어 "," Ngôn ngữ khác"
                };
            using (IMessageQueue b = new ThreadUnsafeQueue(1024))
            {
                for (int i = 0; i < testValues.Length; i++)
                {
                    var c = testValues[i];
                    var testTest = c + c + c;
                    var bytes = Encoding.UTF8.GetBytes(testTest);
                    var len = Toe.CircularArrayQueue.ExtensionMethods.GetByteCount(testTest);
                    Assert.AreEqual(bytes.Length, len, string.Format("Wrong size of {0} ({1})", i, testTest));

                    Toe.CircularArrayQueue.ExtensionMethods.WriteStringContent(b, 1, testTest);
                    var res = Toe.CircularArrayQueue.ExtensionMethods.ReadStringContent(b,1);
                    Assert.AreEqual(testTest, res, string.Format("Wrong content of {0} ({1})", i, testTest));
                }
            }
        }

		#endregion

		#region Methods

		private void TestFloat(IMessageQueue q, int pos, float value)
		{
			q.WriteFloat(pos, value);
			Assert.AreEqual(value, q.ReadFloat(pos));
		}

		private void TestInt64(IMessageQueue q, int pos, long value)
		{
			q.WriteInt64(pos, value);
			Assert.AreEqual(value, q.ReadInt64(pos));
		}

		#endregion
	}
}