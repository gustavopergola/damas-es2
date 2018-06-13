using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Scripts.TiposNS;

public class JogadorTestScript {

    [Test]
    public void NewTestScriptSimplePasses222222222222222() {
        // Use the Assert class to test conditions.
        var peca1 = getPecaJogador1();

        Assert.AreEquals(1, peca1);
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }
}
