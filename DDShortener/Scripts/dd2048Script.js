var tileValues = Create2DArray(4);
var iScore = 0;
$(function () {
    GameStart();

    document.addEventListener('keydown', keyDownTextField, false);
});

function keyDownTextField(e) {
    var keyCode = e.keyCode;

    if (keyCode != 37 && keyCode != 38 && keyCode != 39 && keyCode != 40) return;

    var addNew = false;

    //Arror up
    if (keyCode == 38) {
        for (var c = 0; c < 4; c++) {
            for (var r = 0; r < 4; r++) {
                for (var i = r + 1; i < 4; i++) {
                    if (tileValues[i][c] == 0) continue;

                    if (tileValues[r][c] == tileValues[i][c] || tileValues[r][c] == 0) {
                        var bArr = tileValues[r][c] == 0;
                        if (tileValues[r][c] == tileValues[i][c]) iScore += tileValues[r][c] * 2;
                        tileValues[r][c] += tileValues[i][c];
                        tileValues[i][c] = 0;
                        addNew = true;
                        if (bArr) r--;
                        break;
                    } else {
                        if (tileValues[r][c] != 0) {
                            break;
                        }
                    }
                }
            }
        }
    }

    //Arror Down
    if (keyCode == 40) {
        for (var c = 0; c < 4; c++) {
            for (var r = 0; r < 4; r++) {
                for (var i = r + 1; i < 4; i++) {
                    if (tileValues[3 - i][3 - c] == 0) continue;

                    if (tileValues[3 - r][3 - c] == tileValues[3 - i][3 - c] || tileValues[3 - r][3 - c] == 0) {
                        var bArr = tileValues[3 - r][3 - c] == 0;
                        if (tileValues[3 - r][3 - c] == tileValues[3 - i][3 - c]) iScore += tileValues[3 - r][3 - c] * 2;
                        tileValues[3 - r][3 - c] += tileValues[3 - i][3 - c];
                        tileValues[3 - i][3 - c] = 0;
                        addNew = true;
                        if (bArr) r--;
                        break;
                    } else {
                        if (tileValues[3 - r][3 - c] != 0) {
                            break;
                        }
                    }
                }
            }
        }
    }

    //Arror Left
    if (keyCode == 37) {
        for (var r = 0; r < 4; r++) {
            for (var c = 0; c < 4; c++) {
                for (var i = c + 1; i < 4; i++) {
                    if (tileValues[r][i] == 0) continue;

                    if (tileValues[r][c] == tileValues[r][i] || tileValues[r][c] == 0) {
                        var bArr = tileValues[r][c] == 0;
                        if (tileValues[r][c] == tileValues[r][i]) iScore += tileValues[r][c] * 2;
                        tileValues[r][c] += tileValues[r][i];
                        tileValues[r][i] = 0;
                        addNew = true;
                        if (bArr) c--;
                        break;
                    } else {
                        if (tileValues[r][c] != 0) {
                            break;
                        }
                    }
                }
            }
        }
    }

    //Arror Right
    if (keyCode == 39) {
        for (var r = 0; r < 4; r++) {
            for (var c = 0; c < 4; c++) {
                for (var i = c + 1; i < 4; i++) {
                    if (tileValues[3 - r][3 - i] == 0) continue;

                    if (tileValues[3 - r][3 - c] == tileValues[3 - r][3 - i] || tileValues[3 - r][3 - c] == 0) {
                        var bArr = tileValues[3 - r][3 - c] == 0;
                        if (tileValues[3 - r][3 - c] == tileValues[3 - r][3 - i]) iScore += tileValues[3 - r][3 - c] * 2;
                        tileValues[3 - r][3 - c] += tileValues[3 - r][3 - i];
                        tileValues[3 - r][3 - i] = 0;
                        addNew = true;
                        if (bArr) c--;
                        break;
                    } else {
                        if (tileValues[3 - r][3 - c] != 0) {
                            break;
                        }
                    }
                }
            }
        }
    }

    if (addNew) {
        CreateRandom();
        SetTileText();
    }

    if (IsGameOver())
    {
        $('#game-over').modal('show');
    }
}

function IsGameOver() {
    for (var i = 0; i < 4; i++) {
        for (var j = 0; j < 4; j++) {
            if (i - 1 >= 0) {
                if (tileValues[i - 1][j] == tileValues[i][j]) {
                    return;
                }
            }

            if (i + 1 < 4) {
                if (tileValues[i + 1][j] == tileValues[i][j]) {
                    return;
                }
            }

            if (j - 1 >= 0) {
                if (tileValues[i][j - 1] == tileValues[i][j]) {
                    return;
                }
            }

            if (j + 1 < 4) {
                if (tileValues[i][j + 1] == tileValues[i][j]) {
                    return;
                }
            }

            if (tileValues[i][j] == 0) {
                return;
            }
        }
    }

    return true;
}

function GameStart() {
    iScore = 0;
    for (var _r = 0; _r < 4; _r++) {
        for (var _c = 0; _c < 4; _c++) {
            tileValues[_r][_c] = 0;
        }
    }

    CreateRandom();
    CreateRandom();
    SetTileText();
}

function SetTileText() {
    $("#cur-score").text(iScore);
    for (var _r = 0; _r < 4; _r++) {
        for (var _c = 0; _c < 4; _c++) {
            var _text = "";
            if (tileValues[_r][_c] != 0) _text = tileValues[_r][_c];
            $('#pnl-' + _r.toString() + _c.toString()).alterClass('tile-*', 'tile-' + tileValues[_r][_c])
            //$('#tile' + _r.toString() + _c.toString()).hide();
            $('#tile' + _r.toString() + _c.toString()).text(_text);
            //$('#tile' + _r.toString() + _c.toString()).fadeIn('fast');
        }
    }
}


function CreateRandom() {
    var _tile = Math.floor(Math.random() * 16);
    var _r = Math.floor(_tile / 4);
    var _c = _tile - Math.floor(_r * 4);
    while (tileValues[_r][_c] != 0) {
        _tile = Math.floor(Math.random() * 16);
        _r = Math.floor(_tile / 4);
        _c = _tile - Math.floor(_r * 4);
    }

    var _rnd = Math.floor(Math.random() * 20) == 0 ? Math.floor(Math.random() * 15) == 0 ? 8 : 4 : 2;
    tileValues[_r][_c] = _rnd;
}

function Create2DArray(rows) {
    var arr = [];

    for (var i = 0; i < rows; i++) {
        arr[i] = [];
    }

    return arr;
}