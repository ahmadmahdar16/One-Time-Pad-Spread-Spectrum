Module Module1
    Sub Main()
        Console.WriteLine("Algoritma Spread Spectrum")

        '1. Tentukan kalimat yang akan dienkrip
        Console.WriteLine("Masukkan kalimat yang akan dienkrip: ")
        Dim input As String = Console.ReadLine
        Console.WriteLine("")

        '2. Tentukan kata kunci enkripsi yang digunakan
        Console.WriteLine("Masukkan kata kunci enkripsi: ")
        Dim kataKunci As String = Console.ReadLine
        Console.WriteLine("")

        '3. Lakukan inisialisasi variabel yang digunakan oleh metode ini
        'Penjelasan lebih detail tentang fungsi ini dapat dilihat pada penjelasan skrip dibawah ini
        Dim ss As New SpreadSpectrum()

        '4. Lakukan enkripsi kalimat awal menggunakan algoritma ini
        'Penjelasan lebih detail tentang fungsi ini dapat dilihat pada penjelasan skrip dibawah ini (poin 4a - 4e)
        Dim hasilEnkripsi As String = ss.enkripsi(input, kataKunci)
        Console.WriteLine("Hasil enkripsi kalimat input adalah: " & vbCrLf & hasilEnkripsi & vbCrLf)

        '5. Lakukan dekripsi dari kalimat yang telah terenkripsi
        'Penjelasan lebih detail tentang fungsi ini dapat dilihat pada penjelasan skrip dibawah ini (poin 5a - 5f)
        Dim hasilDekripsi As String = ss.dekripsi(hasilEnkripsi, kataKunci)
        Console.WriteLine("Hasil dekripsi dari kalimat terenkripsi adalah: " & vbCrLf & hasilDekripsi & vbCrLf)

        Console.ReadLine()
    End Sub
End Module

Public Class SpreadSpectrum
    Public Function enkripsi(ByVal teks As String, ByVal kataKunci As String) As String
        Try
            Dim pesanSkala4(teks.Length * 4 - 1) As String
            Dim idxPesanSkala4 As Integer = 0

            '4a. Lakukan perhitungan pada masing-masing karakter teks
            For i As Integer = 0 To teks.Length - 1
                'Lakukan konversi karakter kedalam bentuk binary
                Dim tmpTeksBinary As String = StringToBinary(teks.Substring(i, 1))

                ' Lakukan penyebaran pesan dengan skala pengali adalah 4
                ' Sehingga setiap bit akan digandakan sebanyak 4 kali
                ' Sebagai contoh, dengan string sebelumnya 01110100
                ' Maka akan menghasilkan 4 buah string dengan nilai
                ' 00001111
                ' 11111111
                ' 00001111
                ' 00000000
                pesanSkala4(idxPesanSkala4) = tmpTeksBinary.Substring(0, 1) & tmpTeksBinary.Substring(0, 1) & tmpTeksBinary.Substring(0, 1) & tmpTeksBinary.Substring(0, 1) &
                    tmpTeksBinary.Substring(1, 1) & tmpTeksBinary.Substring(1, 1) & tmpTeksBinary.Substring(1, 1) & tmpTeksBinary.Substring(1, 1)
                idxPesanSkala4 += 1

                pesanSkala4(idxPesanSkala4) = tmpTeksBinary.Substring(2, 1) & tmpTeksBinary.Substring(2, 1) & tmpTeksBinary.Substring(2, 1) & tmpTeksBinary.Substring(2, 1) &
                    tmpTeksBinary.Substring(3, 1) & tmpTeksBinary.Substring(3, 1) & tmpTeksBinary.Substring(3, 1) & tmpTeksBinary.Substring(3, 1)
                idxPesanSkala4 += 1

                pesanSkala4(idxPesanSkala4) = tmpTeksBinary.Substring(4, 1) & tmpTeksBinary.Substring(4, 1) & tmpTeksBinary.Substring(4, 1) & tmpTeksBinary.Substring(4, 1) &
                    tmpTeksBinary.Substring(5, 1) & tmpTeksBinary.Substring(5, 1) & tmpTeksBinary.Substring(5, 1) & tmpTeksBinary.Substring(5, 1)
                idxPesanSkala4 += 1

                pesanSkala4(idxPesanSkala4) = tmpTeksBinary.Substring(6, 1) & tmpTeksBinary.Substring(6, 1) & tmpTeksBinary.Substring(6, 1) & tmpTeksBinary.Substring(6, 1) &
                    tmpTeksBinary.Substring(7, 1) & tmpTeksBinary.Substring(7, 1) & tmpTeksBinary.Substring(7, 1) & tmpTeksBinary.Substring(7, 1)
                idxPesanSkala4 += 1
            Next

            '4b. Lakukan perhitungan pseudonoise berdasarkan kata kunci yang telah dikonversi menjadi string berformat 8-bit
            ' Kemudian lakukan operasi bitwise XOR pada masing-masing string tersebut
            ' Sebagai contoh dengan kata kunci "sonny"
            ' maka hasil konversinya adalah
            ' s        = 01110011
            ' o        = 01101111 
            ' s XOR o  = 00011100
            ' n        = 01101110
            '   XOR n  = 01110010
            ' n        = 01101110
            '   XOR n  = 00011100
            ' y        = 01111001
            '   XOR y  = 01100101
            ' Pseudonoise adalah nilai byte dari operasi terakhir.
            ' yaitu nilai byte dari 01100101 = 101
            Dim kataKunciBytes() As Byte = System.Text.ASCIIEncoding.ASCII.GetBytes(kataKunci)
            Dim pseudonoise As Byte = 0
            For i As Integer = 0 To kataKunciBytes.Length - 1
                If i = 0 Then
                    pseudonoise = kataKunciBytes(i)
                Else
                    pseudonoise = pseudonoise Xor kataKunciBytes(i)
                End If
            Next

            '4c. Dapatkan nilai seed acak dengan rumus
            ' seed(n+1) = (a * seed(n) + c) mod m
            ' dengan konstanta a bernilai 17, konstanta c bernilai 7, dan konstanta m bernilai 84
            ' Sebagai contoh dengan nilai pseudonoise diatas maka
            ' seed(1) = (17 * 101 + 7) mod 84 = 44
            ' seed(2) = (17 * 44 + 7) mod 84 = 83
            ' seed(3) = (17 * 83 + 7) mod 84 = 74
            ' seed(4) = (17 * 74+ 7) mod 84 = 5
            Dim daftarSeed(pesanSkala4.Length - 1) As Byte
            For i As Integer = 0 To daftarSeed.Length - 1
                If i = 0 Then
                    daftarSeed(i) = pseudonoise
                Else
                    daftarSeed(i) = (17 * daftarSeed(i - 1) + 7) Mod 84
                End If
            Next

            '4d. Hitung hasil modulasi dari bit pesan dan seed
            ' Yaitu lakukan proses operasi XOR antara bit pesan dengan nilai seed yang telah didapatkan sebelumnya
            ' Sebagai contoh dengan menggunakan 4 buah string diatas
            ' 00001111 XOR (nilai konversi 8-bit dari 44 = 00101100) = 00100011
            ' 11111111 XOR (nilai konversi 8-bit dari 83 = 01010011) = 10101100
            ' 00001111 XOR (nilai konversi 8-bit dari 74 = 01001010) = 01000101
            ' 00000000 XOR (nilai konversi 8-bit dari 5 = 00000101) = 00000101
            Dim hasilModulasi(pesanSkala4.Length - 1) As Byte
            For i As Integer = 0 To hasilModulasi.Length - 1
                Dim intPesanSkala4 As Integer = Convert.ToByte(pesanSkala4(i), 2)
                hasilModulasi(i) = intPesanSkala4 Xor daftarSeed(i)
            Next

            '4e. Dapatkan karakter dari masing-masing bit sebagai jawaban karakter hasil enkripsi
            Return System.Text.Encoding.Default.GetString(hasilModulasi)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    '* Gunakan fungsi ini untuk mengubah String menjadi Binary
    'contoh: teks 'ABC' akan menjadi '010000010100001001000011'
    Public Shared Function StringToBinary(ByVal teks As String) As String
        Dim hasil As String = ""
        For Each C As Char In teks
            hasil &= System.Convert.ToString(AscW(C), 2).PadLeft(8, "0")
        Next
        Return hasil
    End Function

    Public Function dekripsi(ByVal teks As String, ByVal kataKunci As String) As String
        '5a. Dapatkan nilai bit dari masing-masing karakter dalam pesan terenkripsi
        Dim hasilDemodulasi = System.Text.Encoding.Default.GetBytes(teks)

        '5b. Lakukan perhitungan pseudonoise berdasarkan kata kunci yang telah dikonversi menjadi string berformat 8-bit
        ' Kemudian lakukan operasi bitwise XOR pada masing-masing string tersebut
        ' Sebagai contoh dengan kata kunci "sonny"
        ' maka hasil konversinya adalah
        ' s        = 01110011
        ' o        = 01101111 
        ' s XOR o  = 00011100
        ' n        = 01101110
        '   XOR n  = 01110010
        ' n        = 01101110
        '   XOR n  = 00011100
        ' y        = 01111001
        '   XOR y  = 01100101
        ' Pseudonoise adalah nilai byte dari operasi terakhir.
        ' yaitu nilai byte dari 01100101 = 101
        Dim kataKunciBytes() As Byte = System.Text.ASCIIEncoding.ASCII.GetBytes(kataKunci)
        Dim pseudonoise As Byte = 0
        For i As Integer = 0 To kataKunciBytes.Length - 1
            If i = 0 Then
                pseudonoise = kataKunciBytes(i)
            Else
                pseudonoise = pseudonoise Xor kataKunciBytes(i)
            End If
        Next i

        '5c. Dapatkan nilai seed acak dengan rumus
        ' seed(n+1) = (a * seed(n) + c) mod m
        ' dengan konstanta a bernilai 17, konstanta c bernilai 7, dan konstanta m bernilai 84
        ' Sebagai contoh dengan nilai pseudonoise diatas maka
        ' seed(1) = (17 * 101 + 7) mod 84 = 44
        ' seed(2) = (17 * 44 + 7) mod 84 = 83
        ' seed(3) = (17 * 83 + 7) mod 84 = 74
        ' seed(4) = (17 * 74+ 7) mod 84 = 5
        Dim daftarSeed(hasilDemodulasi.Length - 1) As Byte
        For i As Integer = 0 To daftarSeed.Length - 1
            If i = 0 Then
                daftarSeed(i) = pseudonoise
            Else
                daftarSeed(i) = (17 * daftarSeed(i - 1) + 7) Mod 84
            End If
        Next i

        '5d. Lakukan proses demodulasi dari bit modulasi dan seed
        ' Yaitu lakukan proses operasi XOR antara bit modulasi dengan nilai seed yang telah didapatkan sebelumnya
        ' Sebagai contoh dengan menggunakan 4 buah string diatas
        ' 00100011 XOR (nilai konversi 8-bit dari 44 = 00101100) = 00001111
        ' 10101100 XOR (nilai konversi 8-bit dari 83 = 01010011) = 11111111
        ' 01000101 XOR (nilai konversi 8-bit dari 74 = 01001010) = 00001111
        ' 00000101 XOR (nilai konversi 8-bit dari 5 = 00000101) = 00000000
        Dim pesanSkala4(hasilDemodulasi.Length - 1) As Byte
        For i As Integer = 0 To pesanSkala4.Length - 1
            pesanSkala4(i) = hasilDemodulasi(i) Xor daftarSeed(i)
        Next i

        '5e. Lakukan perhitungan pada masing-masing hasil demodulasi
        Dim bytePesan(pesanSkala4.Length / 4 - 1) As Byte
        Dim idxPesan As Integer = 0
        For i As Integer = 0 To pesanSkala4.Length - 1 Step 4
            ' Lakukan perhitungan untuk mendapatkan isi sebenarnya dari hasil demodulasi
            ' Yaitu dengan cara menyusutkan hasil demodulasi dengan skala pembagi 4
            ' Sehingga setiap 4 bit yang ditemukan akan disusutkan menjadi 1 bit saja
            ' Sebagai contoh, dengan hasil perhitungan demodulasi sebelumnya
            ' 00001111
            ' 11111111
            ' 00001111
            ' 00000000
            ' Maka akan menghasilkan sebuah string dengan nilai 01110100
            Dim tmp As String = StringToBinary(ChrW(pesanSkala4(i)))
            Dim tmp2 As String = StringToBinary(ChrW(pesanSkala4(i + 1)))
            Dim tmp3 As String = StringToBinary(ChrW(pesanSkala4(i + 2)))
            Dim tmp4 As String = StringToBinary(ChrW(pesanSkala4(i + 3)))

            Dim tmpPesan As String = tmp.Substring(0, 1) + tmp.Substring(4, 1) +
                tmp2.Substring(0, 1) + tmp2.Substring(4, 1) +
                tmp3.Substring(0, 1) + tmp3.Substring(4, 1) +
                tmp4.Substring(0, 1) + tmp4.Substring(4, 1)

            ' Dapatkan nilai byte dari hasil penyusutan string tersebut
            bytePesan(idxPesan) = Convert.ToByte(tmpPesan, 2)
            idxPesan += 1
        Next i

        '5f. Dapatkan karakter dari masing-masing bit sebagai jawaban karakter hasil dekripsi
        Return System.Text.Encoding.ASCII.GetString(bytePesan)
    End Function

    '* Gunakan fungsi ini untuk mengubah Binary menjadi String
    'contoh: teks '010000010100001001000011' akan menjadi 'ABC'
    Public Shared Function BinaryToString(ByVal teks As String) As String
        Dim hasil As String = ""

        For i As Integer = 0 To teks.Length - 1 Step 8
            Dim tmpBinary As String = teks.Substring(i, 8)
            hasil += ChrW(Convert.ToByte(tmpBinary, 2))
        Next

        Return hasil
    End Function
End Class
