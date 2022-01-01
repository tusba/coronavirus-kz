namespace Tusba.Patterns.ChainOfResponsibility
{
	public abstract class BaseHandler<Q, R> : InterfaceHandler<Q, R>
	{
		public BaseHandler<Q, R>? Next { get; private init; }

		protected virtual R? DefaultResult => default(R);

		public BaseHandler(BaseHandler<Q, R>? next = null) => Next = next;

		public virtual R? Handle(Q query)
		{
			if (Next is not null)
			{
				return Next.Handle(query);
			}

			return DefaultResult;
		}
	}
}
