uniform sampler2D noise;

void main()
{
	gl_FragColor = texture2D(noise, gl_TexCoord[0].st);
}
