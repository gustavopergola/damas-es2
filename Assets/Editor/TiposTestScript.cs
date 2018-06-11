using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TiposTestScript : MonoBehaviour{

    [Test]
    public void DeveRetornarAPecaDoJogador1() {
        GameObject pecaSpt;

        pecaSpt.GetComponent<Tipos>();
        // Use the Assert class to test conditions.
        var peca1 = pecaSpt.getPecaJogador1();

        Assert.Equals(1, peca1);
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
