uniform sampler2D noise;

void main()
{
	gl_FragColor = gl_TexCoord[0];texture2D(noise, gl_TexCoord[0].st);
}
