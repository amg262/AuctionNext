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
    async jwt({token, user, account, profile}) {
      console.log('jwt', token, user, account, profile);
      if (user) {
        token.id = user.id;
      }
      return token;
    },
    // async session(session, token) {
    //   session.id = token.id;
    //   return session;
    // }

  }
}

const handler = NextAuth(authOptions);

export {handler as GET, handler as POST};