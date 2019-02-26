// Copyright 2015 the pokefans authors. See copying.md for legal info.
var arcrypt = function () {
    var pad = function (num, size) {
        var s = "000000000" + num;
        return s.substr(s.length - size);
    }

  this.encrypt = function(input, arv3) {
    var myga = new this.ga();
    return this.run(input, arv3, myga.encrypt);
  };
  this.decrypt = function(input, arv3) {
    var myga = new this.ga();
    return this.run(input, arv3, myga.decrypt);
  };
  this.run = function(input, arv3, cryptfunc) {
    var result = "";

    for(var i = 0; i <= (input.length / 19); i++) {
      var tadd = parseInt(input.substr((i * 18), 8), 16);
      var tval = parseInt(input.substr((i * 18) + 9, 8), 16);

      output = cryptfunc(tadd, tval, arv3);

      var encadd = output.substr(0, 8);
      var encval = output.substr(8, 17);

      result += encadd + " " + encval + "\n";
    }

    return result;
  };
  this.ga = function() {

     var wrap32 = function(input) {
      if(input < 0)
      {
        input += 0x100000000;
      }
      if(input > 0xFFFFFFFF) {
        input -= 0x100000000;
      }

      return input;
    };
    this.encrypt = function(tadd, tval, arv3) {
      var r = 0x00000000;
      var rsa = 0x9E3779B9;

      var s = [0x09F4FBBD, 
               0x9681884A, 
               0x352027E9, 
               0xF3DEE5A7];

      if(arv3) {
        s = [0x7AA9648F, 
             0x7FAE6994, 
             0xC0EFAAD5, 
             0x42712C57];
      }

      var t = 0;
      var t2 = 0;

      for(var i = 0; i < 32; i++) {
        r += rsa;
        r = wrap32(r);

        t = (tval << 4) & 0xFFFFFFFF;
        t = wrap32(t);

        t += s[0];
        t = wrap32(t);

        t2 = tval + r;
        t2 = wrap32(t2);

        t ^= t2;
        t = wrap32(t);

        t2 = (tval >> 5) & 0x07FFFFFF;
        t2 = wrap32(t2);

        t2 += s[1];
        t2 = wrap32(t2);

        t ^= t2;
        t = wrap32(t);

        tadd += t;
        tadd = wrap32(tadd);

        t = (tadd << 4) & 0xFFFFFFFF;
        t = wrap32(t);

        t += s[2];
        t = wrap32(t);

        t2 = tadd + r;
        t2 = wrap32(t2);

        t ^= t2;
        t = wrap32(t);

        t2 = (tadd >> 5) & 0x07FFFFFF;
        t2 = wrap32(t2);

        t2 += s[3];
        t2 = wrap32(t2);

        t ^= t2;
        t = wrap32(t);

        tval += t;
        tval = wrap32(tval);
      }

      return pad(tadd.toString(16).toUpperCase(),8) + pad(tval.toString(16).toUpperCase(),8);
    };
    this.decrypt = function(tadd, tval, arv3) {
      var r = 0xC6EF3720;
      var rsa = 0x9E3779B9;

      var s = [0x09F4FBBD, 
         0x9681884A, 
         0x352027E9, 
         0xF3DEE5A7];

      if(arv3) {
        s = [0x7AA9648F, 
             0x7FAE6994, 
             0xC0EFAAD5, 
             0x42712C57];
      }

      var t = 0, t2 = 0;

      for(var i = 0; i < 32; i++) {
        t = (tadd << 4) & 0xFFFFFFFF;
        t = wrap32(t);

        t += s[2];
        t = wrap32(t);

        t2 = tadd + r;
        t2 = wrap32(t2);

        t ^= t2;
        t = wrap32(t);

        t2 = (tadd >> 5) & 0x07FFFFFF;
        t2 = wrap32(t2);

        t2 += s[3];
        t2 = wrap32(t2);

        t ^= t2;
        t = wrap32(t);

        tval -= t;
        tval = wrap32(tval);

        t = (tval << 4) & 0xFFFFFFFF;
        t = wrap32(t);

        t += s[0];
        t = wrap32(t);

        t2 = tval + r;
        t2 = wrap32(t2);

        t ^= t2;
        t = wrap32(t);

        t2 = (tval >> 5) & 0x07FFFFFF;
        t2 = wrap32(t2);

        t2 += s[1];
        t2 = wrap32(t2);

        t ^= t2;
        t = wrap32(t);

        tadd -= t;
        tadd = wrap32(tadd);

        r -= rsa;
        r = wrap32(r);
      }

      return pad(tadd.toString(16).toUpperCase(),8) + pad(tval.toString(16).toUpperCase(),8);
    };
  };
};

// for node.js
if (typeof exports !== 'undefined') {
    if (typeof module !== 'undefined' && module.exports) {
        module.exports = arcrypt;
    }
}