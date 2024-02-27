import NextAuth, {NextAuthOptions} from 'next-auth';
import DuendeIdentityServer6 from "next-auth/providers/duende-identity-server6";

export const authOptions: NextAuthOptions = {
  session: {
    strategy: 'jwt',
  },
  providers: [
    DuendeIdentityServer6({
      id: 'id-server',
      clientId: 'nextApp',
      clientSecret: 'secret',
      issuer: 'http://localhost:5000',
      authorization: {params: {scope: 'openid profile auctionApp'}},
      idToken: true
    })
  ],
  callbacks: {
    async jwt({token, profile}) {
      console.log('jwt', token, profile);
      if (profile) {
        token.username = profile.username
      }
      return token;
    },
    async session({session, token}) {
      if (token) {
        session.user.username = token.username;
      }
      return session;
    }

  }
}

const handler = NextAuth(authOptions);

export {handler as GET, handler as POST};